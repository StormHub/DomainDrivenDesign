using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    public sealed class CommitTest : IClassFixture<EFCoreUnitOfWorkSetup>
    {
        readonly EFCoreUnitOfWorkSetup fixture;

        public CommitTest(EFCoreUnitOfWorkSetup fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task CommittedThrows()
        {
            var id = 1;
            await using (var context = fixture.CreateDbContext())
            {
                var unitOfWork = fixture.CreateUnitOfWork(context);
                var newEntity = new TestEntity(id, nameof(SaveChangesTest));
                await context.AddAsync(newEntity);
                await unitOfWork.CommitAsync();
                unitOfWork.IsCommitted.ShouldBeTrue();

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.CommitAsync());
            }

            await using (var context = fixture.CreateDbContext())
            {
                var entity = await context.Set<TestEntity>().FindAsync(id);
                entity.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task AbandonThrows()
        {
            await using var context = fixture.CreateDbContext();
            var unitOfWork = fixture.CreateUnitOfWork(context);
            await unitOfWork.AbandonAsync();
            unitOfWork.IsAbandoned.ShouldBeTrue();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.CommitAsync());
        }

        [Fact]
        public async Task DisposedThrows()
        {
            await using var context = fixture.CreateDbContext();
            var unitOfWork = fixture.CreateUnitOfWork(context);
            unitOfWork.Dispose();
            unitOfWork.IsDisposed.ShouldBeTrue();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.CommitAsync());
        }
    }
}
