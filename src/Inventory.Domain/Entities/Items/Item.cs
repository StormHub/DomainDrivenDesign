// ReSharper disable UnusedAutoPropertyAccessor.Local
using System;
using Domain.Common.DateTimes;
using Domain.Infrastructure;

namespace Inventory.Domain.Entities.Items
{
    public sealed class Item : IAggregateRoot
    {
        internal Item()
        {
        }

        public static Item Create(string name, decimal price, decimal deliveryFee, IClock clock)
        {
            var _ = clock ?? throw new ArgumentNullException(nameof(clock));

            var item = new Item
            {
                Name = name,
                Price = price,
                DeliveryFee = deliveryFee
            };
            
            item.DomainEvents.Add(new ItemCreatedEvent(item, clock));
            return item;
        }

        public int Id { get; private set; }

        public Guid ExternalId { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public decimal DeliveryFee { get; private set; }

        public int SupplierId { get; private set; }

        public DomainEventsCollection DomainEvents { get; } = new DomainEventsCollection();
    }
}
