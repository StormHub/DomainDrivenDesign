using System;
using System.ComponentModel.DataAnnotations;
using Domain.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Orders.Api.Queries.GetOrders
{
    public sealed class GetOrderQuery : IQuery<GetOrderQueryResponse>
    {
        [FromRoute]
        [Required]
        public Guid OrderId { get; set; }
    }
}
