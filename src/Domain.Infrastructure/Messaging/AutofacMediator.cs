using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Domain.Infrastructure.Repository;
using Serilog;

namespace Domain.Infrastructure.Messaging
{
    sealed class AutofacMediator : IMediator
    {
        readonly ILifetimeScope scope;

        public AutofacMediator(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public async Task Command<TCommand>(
            TCommand command,
            CancellationToken cancellationToken)
            where TCommand : class, ICommand
        {
            Task Handler(TCommand message, CancellationToken token)
            {
                var handler = scope.Resolve<ICommandHandler<TCommand>>();
                return handler.Handle(message, token);
            }

            var unitOfWork = scope.Resolve<IUnitOfWork>();
            ExceptionDispatchInfo exceptionDispatchInfo = null;
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await Handler(command, cancellationToken);
                await unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{@Command}", command);
                await unitOfWork.AbandonAsync();
                exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);
            }
            finally
            {
                stopwatch.Stop();
            }

            if (exceptionDispatchInfo != null)
            {
                Log.Information("{MessageType} terminated. {Elapsed:0.0000} ms",
                    command.GetType().Name, stopwatch.Elapsed.TotalMilliseconds);

                exceptionDispatchInfo.Throw();
            }

            Log.Information("{MessageType} completed. {Elapsed:0.0000} ms",
                command.GetType().Name, stopwatch.Elapsed.TotalMilliseconds);

        }

        public async Task Raise<TDomainEvent>(
            TDomainEvent domainEvent,
            CancellationToken cancellationToken)
            where TDomainEvent : class, IDomainEvent
        {
            var handlers = scope.Resolve<IEnumerable<IDomainEventHandler<TDomainEvent>>>();
            foreach (var handler in handlers)
            {
                await handler.Handle(domainEvent);
            }
        }

        public async Task<TQueryResponse> Query<TQuery, TQueryResponse>(
            TQuery query,
            CancellationToken cancellationToken)
            where TQuery : IQuery<TQueryResponse>
            where TQueryResponse : class, IQueryResponse
        {
            Task<TQueryResponse> Handler(TQuery message, CancellationToken token)
            {
                var handler = scope.Resolve<IQueryHandler<TQuery, TQueryResponse>>();
                return handler.Handle(message, token);
            }

            TQueryResponse response = default;
            ExceptionDispatchInfo exceptionDispatchInfo = null;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                response = await Handler(query, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{@Query}", query);
                exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);
            }
            finally
            {
                stopwatch.Stop();
            }

            if (exceptionDispatchInfo != null)
            {
                Log.Information("{MessageType} terminated. {Elapsed:0.0000} ms",
                    query.GetType().Name, stopwatch.Elapsed.TotalMilliseconds);

                exceptionDispatchInfo.Throw();
            }

            Log.Information("{MessageType} completed. {Elapsed:0.0000} ms",
                query.GetType().Name, stopwatch.Elapsed.TotalMilliseconds);

            return response;
        }
    }
}