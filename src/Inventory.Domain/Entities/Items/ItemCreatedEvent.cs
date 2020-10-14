using System;
using Domain.Common.DateTimes;
using Domain.Infrastructure.Messaging;

namespace Inventory.Domain.Entities.Items
{
    public sealed class ItemCreatedEvent : IDomainEvent
    {
        public ItemCreatedEvent(Item item, IClock clock)
        {
            Item = item;
            Timestamp = clock.UtcNow;
        }

        public Item Item { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
