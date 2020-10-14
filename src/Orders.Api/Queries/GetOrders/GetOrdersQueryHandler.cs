using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Orders.Api.Contracts;
using Orders.Domain.Entities.Orders;

namespace Orders.Api.Queries.GetOrders
{
    internal sealed class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, GetOrdersQueryResponse>
    {
        readonly DbContext dbContext;

        public GetOrdersQueryHandler(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<GetOrdersQueryResponse> Handle(GetOrdersQuery query, CancellationToken token)
        {
            var orders = await dbContext.Set<Order>()
                .Include(x => x.Items)
                .ToArrayAsync(token);

            return new GetOrdersQueryResponse
            {
                Orders = orders
                    .Select(OrderSummaryDto.FromEntity)
                    .ToArray()
            };
        }
    }
}
