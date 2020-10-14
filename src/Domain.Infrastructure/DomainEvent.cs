using System;
using Domain.Infrastructure.Messaging;

namespace Domain.Infrastructure
{
    public abstract class DomainEvent : IDomainEvent
    {
        protected DomainEvent(DateTimeOffset timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTimeOffset Timestamp { get; }
    }
}