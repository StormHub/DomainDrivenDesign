// ReSharper disable UnusedAutoPropertyAccessor.Local

using System.Linq;
using Orders.Domain.Entities.Items;

namespace Orders.Domain.Entities.Orders
{
    public sealed class OrderItem
    {
        internal OrderItem()
        {
        }

        public int OrderId { get; private set; }

        public int LineNumber { get; private set; }

        public int ItemId { get; private set; }

        public decimal Price { get; private set; }

        public static OrderItem Create(Order order, Item item)
        {
            var price = item.Price;
            if (order.Items.Any(x => x.ItemId == item.Id))
            {
                // Apply 10 % discount for more than one same item
                price *= 0.90m;
            }
            
            var lineNumber = order.Items.Count;
            return new OrderItem
            {
                OrderId = order.Id,
                LineNumber = lineNumber,
                ItemId = item.Id,
                Price = price
            };
        }
    }
}
