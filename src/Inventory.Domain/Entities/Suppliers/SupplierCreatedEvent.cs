using System;
using Domain.Common.DateTimes;
using Domain.Infrastructure.Messaging;

namespace Inventory.Domain.Entities.Suppliers
{
    public sealed class SupplierCreatedEvent : IDomainEvent
    {
        public SupplierCreatedEvent(Supplier supplier, IClock clock)
        {
            Supplier = supplier;
            Timestamp = clock.UtcNow;
        }

        public Supplier Supplier { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
