using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.Messaging;
using Inventory.Api.Contracts;
using Inventory.Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Queries.GetItems
{
    sealed class GetItemsQueryHandler : IQueryHandler<GetItemsQuery, GetItemsQueryResponse>
    {
        readonly DbContext dbContext;

        public GetItemsQueryHandler(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<GetItemsQueryResponse> Handle(GetItemsQuery query, CancellationToken token)
        {
            var items = await dbContext.Set<Item>()
                .ToArrayAsync(token);
            return new GetItemsQueryResponse
            {
                Items = items.Select(ItemSummaryDto.FromEntity).ToArray()
            };
        }
    }
}
