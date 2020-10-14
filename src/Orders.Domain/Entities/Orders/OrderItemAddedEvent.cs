using System;
using Domain.Common.DateTimes;
using Domain.Infrastructure.Messaging;

namespace Orders.Domain.Entities.Orders
{
    public sealed class OrderItemAddedEvent : IDomainEvent
    {
        public OrderItemAddedEvent(Order order, OrderItem item, IClock clock)
        {
            Order = order;
            Item = item;
            Timestamp = clock.UtcNow;
        }

        public Order Order { get; }

        public OrderItem Item { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
