using System;
using Domain.Infrastructure.Messaging;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    class AnotherTestDomainEvent : IDomainEvent
    {
        public AnotherTestDomainEvent()
        {
            Timestamp = DateTimeOffset.Now;
        }

        public DateTimeOffset Timestamp { get; }
    }
}
