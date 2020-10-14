using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.Messaging;
using Moq;
using Shouldly;
using Xunit;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    public class EventHandlingTest : IClassFixture<EFCoreUnitOfWorkSetup>
    {
        readonly EFCoreUnitOfWorkSetup fixture;

        public EventHandlingTest(EFCoreUnitOfWorkSetup fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task DispatchesDomainEventsOnCommit()
        {
            await using (var context = fixture.CreateDbContext())
            {
                var unitOfWork = fixture.CreateUnitOfWork(context);

                var newEntity = new TestEntity();
                await context.AddAsync(newEntity);

                newEntity.DomainMethod();

                await unitOfWork.CommitAsync();
                unitOfWork.IsCommitted.ShouldBeTrue();
            }
            
            fixture.EventDispatcher.Verify(x => x.Dispatch<IDomainEvent>(It.IsAny<TestDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            fixture.EventDispatcher.Verify(x => x.Dispatch<IDomainEvent>(It.IsAny<AnotherTestDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DoesNotDispatchDomainEventsOnAbandon()
        {
            await using (var context = fixture.CreateDbContext())
            {
                var unitOfWork = fixture.CreateUnitOfWork(context);

                var newEntity = new TestEntity();
                await context.AddAsync(newEntity);

                newEntity.DomainMethod();

                await unitOfWork.AbandonAsync();
            }

            fixture.EventDispatcher.Verify(x => x.Dispatch<IDomainEvent>(It.IsAny<TestDomainEvent>(), It.IsAny<CancellationToken>()), Times.Never);
            fixture.EventDispatcher.Verify(x => x.Dispatch<IDomainEvent>(It.IsAny<AnotherTestDomainEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
