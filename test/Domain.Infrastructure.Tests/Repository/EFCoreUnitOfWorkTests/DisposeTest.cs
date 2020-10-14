using System;
using System.Threading.Tasks;
using Domain.Infrastructure.Messaging;
using Domain.Infrastructure.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Serilog;
using Xunit;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    public sealed class DisposeTest : IDisposable
    {
        readonly SqliteConnection connection;
        readonly DbContextOptions<TestDbContext> options;
        readonly Mock<IDomainEventDispatcher> eventDispatcher;
        readonly Mock<ILogger> logger;

        public DisposeTest()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new TestDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            eventDispatcher = new Mock<IDomainEventDispatcher>(MockBehavior.Strict);
            logger = new Mock<ILogger>(MockBehavior.Strict);
        }

        [Fact]
        public void WarningIfCommittedOrAbandoned()
        {
            const string ExpectedMessage = "UnitOfWork must be either completed or abandoned before disposing.";
            logger.Setup(x => x.Error(ExpectedMessage));
            using (var dbContext = new TestDbContext(options))
            {
                var unitOfWork = new EFCoreUnitOfWork<TestDbContext>(dbContext, eventDispatcher.Object, logger.Object);
                unitOfWork.Dispose();

                // Dispose again should not log error twice
                unitOfWork.Dispose();
            }

            logger.Verify(x => x.Error(ExpectedMessage), Times.Once);
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}
