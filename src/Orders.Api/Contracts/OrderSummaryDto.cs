using System;
using System.ComponentModel.DataAnnotations;
using Orders.Domain.Entities.Orders;

namespace Orders.Api.Contracts
{
    public sealed class OrderSummaryDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public OrderPriority Priority { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public decimal DeliveryFee { get; set; }

        internal static OrderSummaryDto FromEntity(Order order) => new OrderSummaryDto
        {
            Id = order.ExternalId,
            Priority = order.Priority,
            Total = order.GetTotalPrice(),
            DeliveryFee = order.DeliveryFee
        };
    }
}
