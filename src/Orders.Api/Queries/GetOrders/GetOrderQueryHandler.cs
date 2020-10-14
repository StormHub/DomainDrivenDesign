using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.Exceptions;
using Domain.Infrastructure.Messaging;
using Domain.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Orders.Api.Contracts;
using Orders.Domain.Entities.Items;
using Orders.Domain.Entities.Orders;

namespace Orders.Api.Queries.GetOrders
{
    internal sealed class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, GetOrderQueryResponse>
    {
        readonly IRepository<Order> repository;
        readonly DbContext dbContext;

        public GetOrderQueryHandler(IRepository<Order> repository, DbContext dbContext)
        {
            this.repository = repository;
            this.dbContext = dbContext;
        }

        public async Task<GetOrderQueryResponse> Handle(GetOrderQuery query, CancellationToken token)
        {
            var order = (await repository.Query(x => x.ExternalId == query.OrderId, token))
                .SingleOrDefault();
            if (order == null)
            {
                throw EntityNotFoundException.Of<Order>(query.OrderId);
            }

            var itemIds = order.Items.Select(x => x.ItemId).ToList();
            var items = await dbContext.Set<Item>()
                .Where(x => itemIds.Contains(x.Id))
                .ToDictionaryAsync(k => k.Id, v => v, token);

            return new GetOrderQueryResponse
            {
                Order = OrderDto.FromEntity(order, items)
            };
        }
    }
}
