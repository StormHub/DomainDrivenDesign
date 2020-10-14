using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Domain.Infrastructure.Messaging;
using Shouldly;
using Xunit;

namespace Domain.Infrastructure.Tests.Messaging.Mediator
{
    public class WhenAnEventIsRaised
    {
        readonly IContainer container;

        public WhenAnEventIsRaised()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TestEventHandler1>().AsImplementedInterfaces();
            builder.RegisterType<TestEventHandler2>().AsImplementedInterfaces();
            builder.RegisterType<TestInspector>().SingleInstance();
            container = builder.Build();
        }

        [Fact]
        public async Task AllRegisteredHandlersAreInvoked()
        {
            var mediator = new AutofacMediator(container);
            
            await mediator.Raise(new TestEvent(DateTimeOffset.UtcNow), CancellationToken.None);

            var testInspector = container.Resolve<TestInspector>();

            testInspector.FiredHandler1.ShouldBe(true);
            testInspector.FiredHandler2.ShouldBe(true);
        }
    }
}