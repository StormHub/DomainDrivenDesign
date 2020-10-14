using System;
using System.ComponentModel.DataAnnotations;
using Inventory.Domain.Entities.Suppliers;

namespace Inventory.Api.Contracts
{
    public sealed class SupplierDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        public static SupplierDto FromEntity(Supplier supplier) => new SupplierDto
        {
            Id = supplier.ExternalId,
            Name = supplier.Name,
            Phone = supplier.Phone
        };
    }
}
