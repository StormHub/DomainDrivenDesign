using System.Threading.Tasks;
using Domain.Infrastructure.Messaging;

namespace Domain.Infrastructure.Tests.Messaging.Mediator
{
    public class TestEventHandler2 : IDomainEventHandler<TestEvent>
    {
        readonly TestInspector inspector;

        public TestEventHandler2(TestInspector inspector)
        {
            this.inspector = inspector;
        }

        public Task Handle(TestEvent domainEvent)
        {
            inspector.FiredHandler2 = true;
            return Task.CompletedTask;
        }
    }
}