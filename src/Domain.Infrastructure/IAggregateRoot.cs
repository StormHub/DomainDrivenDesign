namespace Domain.Infrastructure
{
    public interface IAggregateRoot : IDomainEntity
    {
        int Id { get; }
    }
}