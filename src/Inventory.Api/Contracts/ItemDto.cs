using System;
using System.ComponentModel.DataAnnotations;
using Inventory.Domain.Entities.Items;
using Inventory.Domain.Entities.Suppliers;

namespace Inventory.Api.Contracts
{
    public sealed class ItemDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal DeliveryFee { get; set; }

        [Required]
        public SupplierDto Supplier { get; set; }

        public static ItemDto FromEntity(Item item, Supplier supplier) => new ItemDto
        {
            Id = item.ExternalId,
            Name = item.Name,
            Price = item.Price,
            DeliveryFee = item.DeliveryFee,
            Supplier = SupplierDto.FromEntity(supplier)
        };
    }
}
