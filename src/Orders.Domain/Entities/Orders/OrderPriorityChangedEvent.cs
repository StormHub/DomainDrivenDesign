using System;
using Domain.Common.DateTimes;
using Domain.Infrastructure.Messaging;

namespace Orders.Domain.Entities.Orders
{
    public sealed class OrderPriorityChangedEvent : IDomainEvent
    {
        public OrderPriorityChangedEvent(Order order, OrderPriority originalPriority, IClock clock)
        {
            Order = order;
            OriginalPriority = originalPriority;
            Timestamp = clock.UtcNow;
        }

        public Order Order { get; }

        public OrderPriority OriginalPriority { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
