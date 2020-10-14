using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace Domain.Data.Repository
{
    public class DbContextLoggerFactory : LoggerFactory
    {
        public DbContextLoggerFactory(Serilog.ILogger logger)
        {
            AddProvider(new SerilogLoggerProvider(logger, false));
        }
    }
}