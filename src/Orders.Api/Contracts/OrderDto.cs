using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Orders.Domain.Entities.Items;
using Orders.Domain.Entities.Orders;

namespace Orders.Api.Contracts
{
    public sealed class OrderDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public OrderPriority Priority { get; set; }

        [Required]
        public decimal DeliveryFee { get; set; }

        [Required]
        public OrderItemDto[] Items { get; set; }

        internal static OrderDto FromEntity(Order order, IReadOnlyDictionary<int, Item> items)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (items == null) throw new ArgumentNullException(nameof(items));

            return new OrderDto
            {
                Id = order.ExternalId,
                Priority = order.Priority,
                DeliveryFee = order.DeliveryFee,
                Items = order.Items.Select(x => OrderItemDto.FromEntity(x, items[x.ItemId])).ToArray()
            };
        }
    }
}
