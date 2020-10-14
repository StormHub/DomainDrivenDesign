using System;
using System.ComponentModel.DataAnnotations;
using Domain.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Queries.GetItems
{
    public sealed class GetItemQuery : IQuery<GetItemQueryResponse>
    {
        [FromRoute]
        [Required]
        public Guid ItemId { get; set; }
    }
}
