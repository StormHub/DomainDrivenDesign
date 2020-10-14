using System;
using System.ComponentModel.DataAnnotations;
using Domain.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Orders.Api.Contracts;

namespace Orders.Api.Commands.CreateOrder
{
    public sealed class CreateOrderCommand : ICommand
    {
        [FromBody]
        [Required]
        public OrderEntryDto[] Entries { get; set; }

        public Guid Id { get; set; }

        public DateTimeOffset OccurredOn { get; set; }
    }
}
