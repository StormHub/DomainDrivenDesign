using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    public sealed class AbandonTest : IClassFixture<EFCoreUnitOfWorkSetup>
    {
        readonly EFCoreUnitOfWorkSetup fixture;

        public AbandonTest(EFCoreUnitOfWorkSetup fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task CommittedThrows()
        {
            var id = Guid.NewGuid();
            await using var context = fixture.CreateDbContext();
            var unitOfWork = fixture.CreateUnitOfWork(context);
            var newEntity = new TestEntity { ExternalId = id, Name = nameof(AbandonTest) };
            await context.AddAsync(newEntity);
            await unitOfWork.CommitAsync();
            unitOfWork.IsCommitted.ShouldBeTrue();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.AbandonAsync());
        }

        [Fact]
        public async Task Abandon()
        {
            await using var context = fixture.CreateDbContext();
            var unitOfWork = fixture.CreateUnitOfWork(context);
            await unitOfWork.AbandonAsync();
            unitOfWork.IsAbandoned.ShouldBeTrue();

            // Abandon again should be fine
            await unitOfWork.AbandonAsync();
        }

        [Fact]
        public async Task DisposedThrows()
        {
            await using var context = fixture.CreateDbContext();
            var unitOfWork = fixture.CreateUnitOfWork(context);
            unitOfWork.Dispose();
            unitOfWork.IsDisposed.ShouldBeTrue();

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await unitOfWork.AbandonAsync());
        }
    }
}
