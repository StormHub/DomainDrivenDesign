using System;

namespace Domain.Infrastructure.Tests.Messaging.Mediator
{
    public class TestEvent : DomainEvent
    {
        public TestEvent(DateTimeOffset occurredOn)
            : base(occurredOn)
        {
        }
    }
}