using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Domain.Infrastructure.Repository
{
    public sealed class EFCoreUnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext 
   {
        readonly TContext dbContext;
        readonly IDomainEventDispatcher domainEventDispatcher;
        readonly ILogger log;

        const int ActivatedState = 0;
        const int CommittedState = 1;
        const int AbandonedState = 2;
        const int DisposedState = 3;

        volatile int state = ActivatedState;

        public EFCoreUnitOfWork(TContext dbContext, IDomainEventDispatcher domainEventDispatcher, ILogger log)
        {
            this.dbContext = dbContext;
            this.domainEventDispatcher = domainEventDispatcher;
            this.log = log;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfNotActive();
            if (dbContext.Database.CurrentTransaction == null)
            {
                throw new InvalidOperationException($"{nameof(SaveChangesAsync)} can only be called in the context of {nameof(ExecuteInTransactionAsync)}");
            }

            await SaveChanges(cancellationToken);
        }

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<CancellationToken, Task<TResult>> operation, CancellationToken cancellationToken = default)
        {
            ThrowIfNotActive();
            try
            {
                var strategy = dbContext.Database.CreateExecutionStrategy();
                return await strategy.ExecuteAsync(
                    async token =>
                    {
                        TResult result;
                        await using (var transaction = await dbContext.Database.BeginTransactionAsync(token))
                        {
                            result = await operation(token);
                            await SaveChanges(cancellationToken);
                            await transaction.CommitAsync(token);
                        }

                        state = CommittedState;
                        log.Debug("UnitOfWork committed with transaction");
                        return result;
                    },
                    cancellationToken);
            }
            catch (Exception e)
            {
                log.Error(e, "UnitOfWork with transaction aborted");
                Abandon();
                throw;
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfNotActive();
            if (dbContext.Database.CurrentTransaction != null)
            {
                throw new InvalidOperationException($"{nameof(CommitAsync)} cannot be called in transaction. If transaction is required, call {nameof(ExecuteInTransactionAsync)} instead.");
            }
            try
            {
                await SaveChanges(cancellationToken);
                state = CommittedState;
                log.Debug("UnitOfWork committed");
            }
            catch (Exception e)
            {
                log.Error(e, "UnitOfWork aborted");
                Abandon();
                throw;
            }
        }

        async Task SaveChanges(CancellationToken cancellationToken)
        {
            await FlushDomainEvents(cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            log.Debug("UnitOfWork save changes");
        }

        async Task FlushDomainEvents(CancellationToken token)
        {
            var passCount = 0;
            // Handling of events could Raise more events, so keep going until nothing left
            while (true)
            {
                passCount++;
                var domainEventsThisPass = dbContext.ChangeTracker
                    .Entries()
                    .Select(e => e.Entity)
                    .OfType<IDomainEntity>()
                    .SelectMany(e => e.DomainEvents.GetAndClear())
                    .ToArray();
                if (domainEventsThisPass.Length == 0) break;

                foreach (var domainEvent in domainEventsThisPass)
                {
                    await domainEventDispatcher.Dispatch(domainEvent, token);
                }

                log.Information("Flushed domain events; pass {Pass}, {EventCount} events", passCount, domainEventsThisPass.Length);
            }
        }

        public Task AbandonAsync()
        {
            Abandon();
            return Task.CompletedTask;
        }

        void Abandon()
        {
            var currentState = state;
            switch (currentState)
            {
                case CommittedState:
                    throw new InvalidOperationException("UnitOfWork is already committed");
                case DisposedState:
                    throw new ObjectDisposedException("UnitOfWork is already disposed");
                case AbandonedState:
                    return;
                default:
                    state = AbandonedState;
                    log.Information("UnitOfWork abandoned");
                    break;
            }
        }

        void ThrowIfNotActive()
        {
            var currentState = state;
            if (currentState > ActivatedState)
            {
                throw new InvalidOperationException($"Unit of work is not active state={state}");
            }
        }

        internal bool IsCommitted => state == CommittedState;

        internal bool IsAbandoned => state == AbandonedState;

        internal bool IsDisposed => state == DisposedState;

        void Dispose(bool disposing)
        {
            var currentState = state;
            if (currentState == DisposedState || !disposing)
            {
                return;
            }
            if (currentState == ActivatedState)
            {
                log.Error("UnitOfWork must be either completed or abandoned before disposing.");
            }
            state = DisposedState;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EFCoreUnitOfWork()
        {
            Dispose(false);
        }
    }
}