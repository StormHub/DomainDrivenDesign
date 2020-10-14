using System;
using System.Threading.Tasks;
using Domain.WebApi.Bootstrappers;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Domain.WebApi
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            AppHostBuilder.InitializeLogger(args);

            try
            {
                using var host = AppHostBuilder.Create(args).Build();
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                AppHostBuilder.FlushLogger();
            }
        }
    }
}
