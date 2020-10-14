using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Linq;
using Serilog;

namespace Domain.Data.Migrations
{
    public sealed class DbUpgradeManager
    {
        readonly IDbUpgrader dbUpgrader;
        readonly SqlAppLockDatabaseLock.Factory dbLockFactory;
        readonly ILogger log;

        readonly NamedLocks locks = new NamedLocks();
        readonly ConcurrentBag<string> upgradedDatabases = new ConcurrentBag<string>();

        public DbUpgradeManager(
            IDbUpgrader dbUpgrader,
            SqlAppLockDatabaseLock.Factory dbLockFactory, // factory used to defer instantiation until we actually need it
            ILogger log)
        {
            this.dbUpgrader = dbUpgrader;
            this.dbLockFactory = dbLockFactory;
            this.log = log;
        }

        public void EnsureDbIsUpgraded(string connectionString)
        {
            var databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
            if (upgradedDatabases.Contains(databaseName))
            {
                return;
            }

            var lockName = connectionString;
            
            lock (locks.GetLock(lockName))
            {
                if (upgradedDatabases.Contains(databaseName)) return;

                using (dbLockFactory(connectionString))
                {                    
                    log.Debug("Database '{DatabaseName}' upgrade starting", databaseName);
                    dbUpgrader.Run(connectionString);
                    upgradedDatabases.Add(databaseName);
                    log.Debug("Database '{DatabaseName}' upgrade completed", databaseName);
                }
            }
        }
    }
}