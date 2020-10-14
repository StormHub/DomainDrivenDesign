using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common.DateTimes;
using Domain.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities.Items;
using Orders.Domain.Entities.Orders;

namespace Orders.Domain.Handlers.WhenOrderItemAdded
{
    public sealed class ApplyOrderPriority : IDomainEventHandler<OrderItemAddedEvent>
    {
        readonly IClock clock;
        readonly DbContext dbContext;

        public ApplyOrderPriority(DbContext dbContext, IClock clock)
        {
            this.dbContext = dbContext;
            this.clock = clock;
        }

        public async Task Handle(OrderItemAddedEvent domainEvent)
        {
            var _ = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));

            var order = domainEvent.Order;

            // Total price on original price
            var itemIds = order.Items.Select(x => x.ItemId)
                .Distinct()
                .ToList();

            var items = await dbContext.Set<Item>()
                .Where(x => itemIds.Contains(x.Id))
                .ToDictionaryAsync(k => k.Id, v => v);

            var total = order.Items.Sum(item => items[item.ItemId].Price);

            OrderPriority priority;
            if (total > 3000)
            {
                priority = OrderPriority.High;
            }
            else if (total > 1000)
            {
                priority = OrderPriority.Normal;
            }
            else
            {
                priority = OrderPriority.Low;
            }

            domainEvent.Order.ChangePriority(priority, clock);
        }
    }
}
