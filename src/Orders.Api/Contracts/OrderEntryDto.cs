using System;
using System.ComponentModel.DataAnnotations;

namespace Orders.Api.Contracts
{
    public sealed class OrderEntryDto
    {
        [Required]
        public Guid ItemId { get; set; }
    }
}
