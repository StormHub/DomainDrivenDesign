using System;
using Domain.Common.DateTimes;
using Domain.Infrastructure.Messaging;

namespace Orders.Domain.Entities.Orders
{
    public sealed class OrderCreatedEvent : IDomainEvent
    {
        public OrderCreatedEvent(Order order, IClock clock)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Timestamp = clock.UtcNow;
        }

        public Order Order { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
