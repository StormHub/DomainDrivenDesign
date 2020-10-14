using System.Threading;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Messaging
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch<T>(T domainEvent, CancellationToken token)
            where T : class, IDomainEvent;
    }
}