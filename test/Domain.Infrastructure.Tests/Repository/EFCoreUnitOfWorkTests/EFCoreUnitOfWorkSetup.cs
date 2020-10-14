using System;
using Domain.Infrastructure.Messaging;
using Domain.Infrastructure.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Serilog;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    public sealed class EFCoreUnitOfWorkSetup : IDisposable
    {
        readonly SqliteConnection connection;
        readonly DbContextOptions<TestDbContext> options;

        public EFCoreUnitOfWorkSetup()
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

            EventDispatcher = new Mock<IDomainEventDispatcher>();
            Logger = new Mock<ILogger>();
        }

        public Random Rand => new Random();

        internal Mock<IDomainEventDispatcher> EventDispatcher { get; }


        internal Mock<ILogger> Logger { get; }

        internal TestDbContext CreateDbContext() => new TestDbContext(options);

        internal EFCoreUnitOfWork<TestDbContext> CreateUnitOfWork(TestDbContext dbContext) =>  new EFCoreUnitOfWork<TestDbContext>(dbContext, EventDispatcher.Object, Logger.Object);

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
