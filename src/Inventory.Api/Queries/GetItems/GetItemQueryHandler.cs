using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.Exceptions;
using Domain.Infrastructure.Messaging;
using Domain.Infrastructure.Repository;
using Inventory.Api.Contracts;
using Inventory.Domain.Entities.Items;
using Inventory.Domain.Entities.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Queries.GetItems
{
    sealed class GetItemQueryHandler : IQueryHandler<GetItemQuery, GetItemQueryResponse>
    {
        readonly DbContext dbContext;
        readonly IRepository<Item> repository;

        public GetItemQueryHandler(DbContext dbContext, IRepository<Item> repository)
        {
            this.dbContext = dbContext;
            this.repository = repository;
        }

        public async Task<GetItemQueryResponse> Handle(GetItemQuery query, CancellationToken token)
        {
            var item = (await repository.Query(x => x.ExternalId == query.ItemId, token))
                .SingleOrDefault();
            if (item == null)
            {
                throw EntityNotFoundException.Of<Item>(query.ItemId);
            }

            var supplier = await dbContext.Set<Supplier>()
                .FindAsync(new object[] { item.SupplierId }, token);

            return new GetItemQueryResponse
            {
                Item = ItemDto.FromEntity(item, supplier)
            };
        }
    }
}
