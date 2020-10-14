using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Domain.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Serilog;

namespace Domain.Data.Repository
{
    internal sealed class AppDbRepository<TAggregateRoot> : EFCoreRepository<AppDbContext, TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IReadOnlyCollection<INavigation> navigationProperties;

        public AppDbRepository(AppDbContext dbContext, ILogger log)
            : base(dbContext, log)
        {
            navigationProperties = dbContext.Model.FindEntityType(typeof(TAggregateRoot)).GetNavigations().ToList();
        }

        protected override IQueryable<TAggregateRoot> Fetch(IQueryable<TAggregateRoot> dbSet)
        {
            var query = dbSet;
            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty.Name);
            }
            return query;
        }
    }
}
