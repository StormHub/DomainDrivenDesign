using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Repository
{
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        Task<TAggregateRoot> Find(int ids);

        Task<TAggregateRoot[]> Find(params int[] ids);

        Task<TAggregateRoot> Find(Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> query, CancellationToken token);

        Task<TAggregateRoot[]> Query(Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> query, CancellationToken token);

        Task<TAggregateRoot[]> Query(Expression<Func<TAggregateRoot, bool>> query, CancellationToken token);

        Task Add(TAggregateRoot entry, CancellationToken token);

        Task Remove(TAggregateRoot entry);
    }
}