using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities.Items;
using Orders.Domain.Entities.Orders;

namespace Orders.Domain.Handlers.WhenOrderPriorityChanged
{
    public sealed class CalculateDeliveryFee : IDomainEventHandler<OrderPriorityChangedEvent>
    {
        readonly DbContext dbContext;

        public CalculateDeliveryFee(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Handle(OrderPriorityChangedEvent domainEvent)
        {
            var _ = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));

            var order = domainEvent.Order;
            var itemIds = order.Items
                .Select(x => x.ItemId)
                .Distinct()
                .ToList();

            var items = await dbContext.Set<Item>()
                .Where(x => itemIds.Contains(x.Id))
                .ToListAsync();
            var fee = items.Sum(x => x.DeliveryFee);

            var percent = order.Priority switch
            {
                OrderPriority.High => 0.1m,
                OrderPriority.Normal => 0.05m,
                OrderPriority.Low => 0m,
                _ => 0m
            };

            fee += fee * percent;

            order.ApplyDeliveryFee(fee);
        }
    }
}
