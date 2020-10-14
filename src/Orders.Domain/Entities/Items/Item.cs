// ReSharper disable UnusedAutoPropertyAccessor.Local
using System;

namespace Orders.Domain.Entities.Items
{
    public sealed class Item
    {
        internal Item()
        {
        }

        public int Id { get; private set; }

        public Guid ExternalId { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public decimal DeliveryFee { get; private set; }
    }
}
