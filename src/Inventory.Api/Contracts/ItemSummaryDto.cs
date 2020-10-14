using System;
using System.ComponentModel.DataAnnotations;
using Inventory.Domain.Entities.Items;

namespace Inventory.Api.Contracts
{
    public sealed class ItemSummaryDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public static ItemSummaryDto FromEntity(Item item) =>
            new ItemSummaryDto
            {
                Id = item.ExternalId,
                Name = item.Name,
                Price = item.Price
            };
    }
}
