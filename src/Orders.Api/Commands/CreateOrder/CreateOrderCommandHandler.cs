using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common.DateTimes;
using Domain.Infrastructure.Exceptions;
using Domain.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities.Items;
using Orders.Domain.Entities.Orders;

namespace Orders.Api.Commands.CreateOrder
{
    sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        readonly DbContext dbContext;
        readonly IClock clock;

        public CreateOrderCommandHandler(DbContext dbContext, IClock clock)
        {
            this.dbContext = dbContext;
            this.clock = clock;
        }

        public async Task Handle(CreateOrderCommand command, CancellationToken token)
        {
            var order = Order.Create(clock);
            command.Id = order.ExternalId;

            var itemIds = command.Entries.Select(x => x.ItemId).ToList();
            var items = await dbContext.Set<Item>()
                .Where(x => itemIds.Contains(x.ExternalId))
                .ToDictionaryAsync(k => k.ExternalId, v => v, token);

            foreach (var entry in command.Entries)
            {
                if (!items.TryGetValue(entry.ItemId, out var item))
                {
                    throw EntityNotFoundException.Of<Item>(entry.ItemId);
                }
                order.Add(item, clock);
            }

            await dbContext.AddAsync(order, token);
        }
    }
}
