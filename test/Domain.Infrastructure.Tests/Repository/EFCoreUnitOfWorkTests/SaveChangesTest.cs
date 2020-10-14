using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    public sealed class SaveChangesTest : IClassFixture<EFCoreUnitOfWorkSetup>
    {
        readonly EFCoreUnitOfWorkSetup fixture;

        public SaveChangesTest(EFCoreUnitOfWorkSetup fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task CommitTransaction()
        {
            var id = fixture.Rand.Next();
            await using (var context = fixture.CreateDbContext())
            {
                var unitOfWork = fixture.CreateUnitOfWork(context);
                await unitOfWork.ExecuteInTransactionAsync(
                    async token =>
                    {
                        var newEntity = new TestEntity(id, nameof(SaveChangesTest));
                        await context.AddAsync(newEntity, token);
                        await unitOfWork.SaveChangesAsync(token);
                        return newEntity;
                    });

                unitOfWork.IsCommitted.ShouldBeTrue();
            }

            await using (var context = fixture.CreateDbContext())
            {
                var entity = await context.Set<TestEntity>().FindAsync(id);
                entity.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task RollbackTransaction()
        {
            var id = fixture.Rand.Next();
            await using (var context = fixture.CreateDbContext())
            {
                var unitOfWork = fixture.CreateUnitOfWork(context);
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.ExecuteInTransactionAsync(
                    async token =>
                    {
                        var newEntity = new TestEntity(id, nameof(SaveChangesTest));
                        await context.AddAsync(newEntity, token);
                        await unitOfWork.SaveChangesAsync(token);

                        throw new InvalidOperationException();
#pragma warning disable 162
                        return newEntity;
#pragma warning restore 162
                    }));

                unitOfWork.IsAbandoned.ShouldBeTrue();
            }

            await using (var context = fixture.CreateDbContext())
            {
                var entity = await context.Set<TestEntity>().FindAsync(id);
                entity.ShouldBeNull();
            }
        }

        [Fact]
        public async Task NoTransactionThrows()
        {
            await using var context = fixture.CreateDbContext();
            var unitOfWork = fixture.CreateUnitOfWork(context);
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.SaveChangesAsync());
        }

        [Fact]
        public async Task CommittedThrows()
        {
            var id = fixture.Rand.Next();
            await using (var context = fixture.CreateDbContext())
            {
                var unitOfWork = fixture.CreateUnitOfWork(context);
                var newEntity = new TestEntity(id, nameof(SaveChangesTest));
                await context.AddAsync(newEntity);
                await unitOfWork.CommitAsync();
                unitOfWork.IsCommitted.ShouldBeTrue();

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.SaveChangesAsync());
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

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.SaveChangesAsync());
        }

        [Fact]
        public async Task DisposedThrows()
        {
            await using var context = fixture.CreateDbContext();
            var unitOfWork = fixture.CreateUnitOfWork(context);
            unitOfWork.Dispose();
            unitOfWork.IsDisposed.ShouldBeTrue();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.SaveChangesAsync());
        }
    }
}
