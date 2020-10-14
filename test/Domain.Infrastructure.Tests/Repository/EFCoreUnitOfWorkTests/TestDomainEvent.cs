using System;
using Domain.Infrastructure.Messaging;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    sealed class TestDomainEvent : IDomainEvent
    {
        public TestDomainEvent()
        {
            Timestamp = DateTimeOffset.Now;
        }

        public DateTimeOffset Timestamp { get; }
    }
}
