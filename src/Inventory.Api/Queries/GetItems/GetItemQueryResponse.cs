using Domain.Infrastructure.Messaging;
using Inventory.Api.Contracts;

namespace Inventory.Api.Queries.GetItems
{
    public sealed class GetItemQueryResponse : IQueryResponse
    {
        public ItemDto Item { get; set; }
    }
}
