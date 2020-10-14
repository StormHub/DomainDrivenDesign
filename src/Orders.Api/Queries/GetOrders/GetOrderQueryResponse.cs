using Domain.Infrastructure.Messaging;
using Orders.Api.Contracts;

namespace Orders.Api.Queries.GetOrders
{
    public sealed class GetOrderQueryResponse : IQueryResponse
    {
        public OrderDto Order { get; set; }
    }
}
