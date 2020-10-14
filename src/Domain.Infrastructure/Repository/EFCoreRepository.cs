using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Domain.Infrastructure.Repository
{
    public class EFCoreRepository<TContext, TAggregateRoot> : IRepository<TAggregateRoot>
        where TContext : DbContext
        where TAggregateRoot : class, IAggregateRoot
    {
        public EFCoreRepository(DbContext dbContext, ILogger log)
        {
            Context = dbContext;
            Logger = log;
        }

        protected DbContext Context { get; }

        protected ILogger Logger { get; }

        public async Task<TAggregateRoot> Find(int id)
        {
            var entity = await Fetch(Context.Set<TAggregateRoot>())
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();
            if (entity == null)
            {
                throw EntityNotFoundException.Of<TAggregateRoot>(id);
            }

            return entity;
        }

        public async Task<TAggregateRoot[]> Find(params int[] ids)
        {
            var entities = await Fetch(Context.Set<TAggregateRoot>())
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync();

            var missingIds = ids.Except(entities.Select(x => x.Id)).ToArray();
            if (missingIds.Any())
            {
                throw EntityNotFoundException.Of<TAggregateRoot>()
                    .WithData("missingIds", string.Join(",", missingIds));
            }

            return entities;
        }

        public async Task<TAggregateRoot> Find(Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> query, CancellationToken token)
        {
            var result = query.Invoke(Context.Set<TAggregateRoot>());
            var entity = await Fetch(result).FirstOrDefaultAsync(token);
            return entity;
        }

        public async Task<TAggregateRoot[]> Query(Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> query, CancellationToken token)
        {
            var results = query.Invoke(Context.Set<TAggregateRoot>());
            var entities = await Fetch(results).ToArrayAsync(token);
            return entities;
        }

        public async Task<TAggregateRoot[]> Query(Expression<Func<TAggregateRoot, bool>> query, CancellationToken token)
        {
            var entities = await Fetch(Context.Set<TAggregateRoot>())
                .Where(query)
                .ToArrayAsync(token);
            return entities;
        }

        public async Task Add(TAggregateRoot entity, CancellationToken token)
        {
            await Context.AddAsync(entity, token);
        }

        public Task Remove(TAggregateRoot entry)
        {
            Context.Remove(entry);
            return Task.CompletedTask;
        }

        protected virtual IQueryable<TAggregateRoot> Fetch(IQueryable<TAggregateRoot> dbSet) => dbSet;
    }
}