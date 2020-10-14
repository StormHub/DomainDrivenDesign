namespace Domain.Infrastructure
{
    public interface IDomainEntity
    {
        DomainEventsCollection DomainEvents { get; }
    }
}