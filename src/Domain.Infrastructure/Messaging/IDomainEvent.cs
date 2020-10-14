using System;

namespace Domain.Infrastructure.Messaging
{
    public interface IDomainEvent
    {
        DateTimeOffset Timestamp { get; }
    }
}