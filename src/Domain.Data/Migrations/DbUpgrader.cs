using System;
using DbUp;
using DbUp.Engine;
using DbUp.Support;
using Serilog;

namespace Domain.Data.Migrations
{
    sealed class DbUpgrader : IDbUpgrader
    {
        readonly ILogger logger;

        public DbUpgrader(ILogger logger)
        {
            this.logger = logger;
        }

        static UpgradeEngine CreateEngine(string connectionString) => DeployChanges.To
            .SqlDatabase(connectionString)
            .JournalToSqlTable("dbo", nameof(SchemaVersions))
            .WithScriptsEmbeddedInAssembly(typeof(DbUpgrader).Assembly, 
                script => script.StartsWith("Domain.Data.Migrations.Scripts.", StringComparison.InvariantCultureIgnoreCase), 
                new SqlScriptOptions { ScriptType = ScriptType.RunOnce })
            .WithExecutionTimeout(TimeSpan.FromMinutes(15))
            .LogToAutodetectedLog()
            .WithTransactionPerScript()
            .Build();

        public void Run(string connectionString)
        {
            logger.Debug("Migrating database");
            var upgradeEngine = CreateEngine(connectionString);
            var result = upgradeEngine.PerformUpgrade();
            if (!result.Successful)
            {
                throw result.Error;
            }
        }
    }
}