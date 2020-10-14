using Domain.Common.Configurations;
using Domain.Data.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data.Repository
{
    sealed class DbContextOptionsInitializer
    {
        readonly DatabaseSettings databaseSettings;
        readonly DbUpgradeManager upgradeManager;
        readonly DbContextLoggerFactory loggerFactory;

        public DbContextOptionsInitializer(
            DatabaseSettings databaseSettings,
            DbUpgradeManager upgradeManager,
            DbContextLoggerFactory loggerFactory)
        {
            this.databaseSettings = databaseSettings;
            this.upgradeManager = upgradeManager;
            this.loggerFactory = loggerFactory;
        }

        public DbContextOptions<TContext> Create<TContext>()
            where TContext : DbContext
        {
            var connectionString = databaseSettings.ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<TContext>()
                .UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                    upgradeManager.EnsureDbIsUpgraded(connectionString);
                })
                .UseLoggerFactory(loggerFactory);
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif

            return optionsBuilder.Options;
        }
    }
}