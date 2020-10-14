// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common.DateTimes;
using Domain.Infrastructure;
using Domain.Infrastructure.Exceptions;
using Orders.Domain.Entities.Items;

namespace Orders.Domain.Entities.Orders
{
    public sealed class Order : IAggregateRoot
    {
        internal Order()
        {
        }

        public static Order Create(IClock clock)
        {
            var order = new Order
            {
                ExternalId = Guid.NewGuid(),
                Priority = OrderPriority.Normal,
                DeliveryFee = 0m
            };
            order.DomainEvents.Add(new OrderCreatedEvent(order, clock));

            return order;
        }

        public int Id { get; private set; }

        public Guid ExternalId { get; private set; }

        public OrderPriority Priority { get; private set; }

        public decimal DeliveryFee { get; private set; }

        public List<OrderItem> Items { get; private set; } = new List<OrderItem>();

        public decimal GetTotalPrice() => Items.Sum(x => x.Price);

        public void ChangePriority(OrderPriority priority, IClock clock)
        {
            if (Priority == priority)
            {
                return;
            }

            var originalPriority = Priority;
            Priority = priority;
            DomainEvents.Add(new OrderPriorityChangedEvent(this, originalPriority, clock));
        }

        public void Add(Item item, IClock clock)
        {
            var _ = item ?? throw new ArgumentNullException(nameof(item));

            var orderItem = OrderItem.Create(this, item);
            Items.Add(orderItem);

            DomainEvents.Add(new OrderItemAddedEvent(this, orderItem, clock));
        }

        public void ApplyDeliveryFee(decimal fee)
        {
            if (fee < 0)
            {
                throw DomainDataRuleException.Of<Order>("Fee must be greater or equal to zero", nameof(DeliveryFee));
            }

            DeliveryFee = fee;
        }

        public DomainEventsCollection DomainEvents { get; } = new DomainEventsCollection();
    }
}
