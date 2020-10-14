// ReSharper disable UnusedAutoPropertyAccessor.Local
using System;
using Domain.Common.DateTimes;
using Domain.Infrastructure;

namespace Inventory.Domain.Entities.Suppliers
{
    public sealed class Supplier : IDomainEntity
    {
        internal Supplier()
        {
        }

        public static Supplier Create(string name, string phone, IClock clock)
        {
            var _ = clock ?? throw new ArgumentNullException(nameof(clock));

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(name)} cannot be empty or null");
            }

            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentException($"{nameof(phone)} cannot be empty or null");
            }

            var supplier = new Supplier
            {
                ExternalId = Guid.NewGuid(),
                Name = name,
                Phone = phone
            };

            supplier.DomainEvents.Add(new SupplierCreatedEvent(supplier, clock));
            return supplier;
        }

        public int Id { get; private set; }

        public Guid ExternalId { get; private set; }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public DomainEventsCollection DomainEvents { get; } = new DomainEventsCollection();
    }
}
