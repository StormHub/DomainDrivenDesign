using System;
using System.ComponentModel.DataAnnotations;
using Orders.Domain.Entities.Items;
using Orders.Domain.Entities.Orders;

namespace Orders.Api.Contracts
{
    public sealed class OrderItemDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        internal static OrderItemDto FromEntity(OrderItem orderItem, Item item)
        {
            if (orderItem == null) throw new ArgumentNullException(nameof(orderItem));
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (orderItem.ItemId != item.Id)
            {
                throw new ArgumentException($"{nameof(OrderItem)} {orderItem.ItemId} must be the same as {nameof(Item)} {item.Id}");
            }

            return new OrderItemDto
            {
                Id = item.ExternalId,
                Name = item.Name,
                Price = orderItem.Price,
            };
        }
    }
}
