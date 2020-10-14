using System.ComponentModel.DataAnnotations;
using Domain.Infrastructure.Messaging;
using Inventory.Api.Contracts;

namespace Inventory.Api.Queries.GetItems
{
    public sealed class GetItemsQueryResponse : IQueryResponse
    {
        [Required]
        public ItemSummaryDto[] Items { get; set; }
    }
}
