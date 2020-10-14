using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Serilog;

namespace Domain.Data.Migrations
{
    public sealed class SqlAppLockDatabaseLock : IDatabaseLock
    {
        // autofac delegate factory
        // see: http://autofaccn.readthedocs.io/en/latest/advanced/delegate-factories.html
        // used to defer instantiatation until a) config is available or b) we actually need it.  We're using it for the latter.
        public delegate IDatabaseLock Factory(string connectionString);

        // This class toggles the database between SINGLE_USER and MULTI_USER mode
        // https://docs.microsoft.com/en-us/sql/t-sql/statements/alter-database-transact-sql-set-options?view=sql-server-2017        

        const string DbLockName = "DbMigrations.Lock";
        readonly string connectionString;
        readonly ILogger log;
        readonly string databaseName;

        SqlConnection dbConnection;
        bool disposed = false;

        public SqlAppLockDatabaseLock(string connectionString, ILogger log)
        {
            this.connectionString = connectionString;
            this.log = log;
            databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

            GetAppLock();
        }

        void GetAppLock()
        {
            log.Information("Database '{DatabaseName}' requesting lock", databaseName);
            log.Information("Connection string {ConnectionString}", connectionString.Split(new string[] { "Password" }, StringSplitOptions.None)[0]);

            dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            //var sql = "EXEC sp_getapplock @Resource = 'DbMigrations',  @LockOwner='Session', @LockMode = 'Exclusive', @LockTimeout='60000';";
            const string sql = "sp_getapplock";

            var command = new SqlCommand(sql, dbConnection);
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("Resource", DbLockName);
            command.Parameters.AddWithValue("LockOwner", "Session");
            command.Parameters.AddWithValue("LockMode", "Exclusive");
            command.Parameters.AddWithValue("LockTimeout", "60000");

            var returnValue = command.Parameters.Add("ReturnValue", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;
            
            command.ExecuteNonQuery();

            var result = int.Parse(returnValue.Value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture);
            if (result < 0)
            {
                var message = $"Database '{databaseName}' lock error, code '{result}'";
                var exception = new Exception(message);
                log.Error(message);
                throw exception;
            }
            else
            {
                log.Information("Database '{DatabaseName}' lock successful", databaseName);
            }
        }

        void ReleaseAppLock()
        {
            log.Information("Database '{DatabaseName}' releasing lock.", databaseName);

            var sql = "sp_releaseapplock";

            var command = new SqlCommand(sql, dbConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Resource", DbLockName);
            command.Parameters.AddWithValue("LockOwner", "Session");

            command.ExecuteNonQuery();

            log.Information("Database '{DatabaseName}' lock released.", databaseName);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                ReleaseAppLock();
                disposed = true;
            }
        }
    }
}