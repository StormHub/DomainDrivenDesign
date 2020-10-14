using System.Threading.Tasks;
using Domain.Infrastructure.Messaging;

namespace Domain.Infrastructure.Tests.Messaging.Mediator
{
    public class TestEventHandler1 : IDomainEventHandler<TestEvent>
    {
        readonly TestInspector inspector;

        public TestEventHandler1(TestInspector inspector)
        {
            this.inspector = inspector;
        }

        public Task Handle(TestEvent domainEvent)
        {
            inspector.FiredHandler1 = true;
            return Task.CompletedTask;
        }
    }
}