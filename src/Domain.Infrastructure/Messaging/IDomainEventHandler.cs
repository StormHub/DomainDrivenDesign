using System.Threading.Tasks;

namespace Domain.Infrastructure.Messaging
{
    public interface IDomainEventHandler<in T>
        where T : IDomainEvent
    {
        Task Handle(T domainEvent);
    }
}