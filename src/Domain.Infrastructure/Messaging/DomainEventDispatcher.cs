using System.Threading;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Messaging
{
    sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        readonly IMediator mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Dispatch<T>(T domainEvent, CancellationToken token)
            where T : class, IDomainEvent
        {
            // Usage of (dynamic) defers type binding until runtime, allowing the mediator to resolve the handlers for the concrete type
            await ((dynamic)mediator).Raise((dynamic)domainEvent, token);
        }
    }
}