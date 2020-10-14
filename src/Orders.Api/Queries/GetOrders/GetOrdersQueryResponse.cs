using Domain.Infrastructure.Messaging;
using Orders.Api.Contracts;

namespace Orders.Api.Queries.GetOrders
{
    public sealed class GetOrdersQueryResponse : IQueryResponse
    {
        public OrderSummaryDto[] Orders { get; set; }
    }
}
