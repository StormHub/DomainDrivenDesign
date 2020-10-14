using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Common;
using Domain.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Domain.WebApi.Bootstrappers
{
    static class AppHostBuilder
    {
        public static IHostBuilder Create(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureContainer<ContainerBuilder>(
                    (context, builder) =>
                    {
                        builder.ConfigureContainer(context.Configuration);
                    })
                .ConfigureWebHostDefaults(config =>
                    {
                        config.ConfigureKestrel(x => x.AddServerHeader = false)
                            .UseWebRoot("Public")
                            .UseStartup<Startup>();
                    })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((context, services) =>
                {
                    // see: https://docs.microsoft.com/en-us/ef/core/miscellaneous/logging
                    services.AddSingleton<DbContextLoggerFactory>();
                })
                .UseSerilog();

            return hostBuilder;
        }

        public static void InitializeLogger(string[] args)
        {
            var loggerBuilder = new LoggerConfiguration();

            // We use the default host builder behaviour to load all
            // configurations e.g. appsetting, appsettings.Env and 
            // environment variable overrides to default the Serilog
            // configurations during startup.

            using (var host = Host.CreateDefaultBuilder(args).Build())
            {
                var configuration = host.Services.GetRequiredService<IConfiguration>();
                if (configuration.GetSection("Serilog").Exists())
                {
                    loggerBuilder = loggerBuilder.ReadFrom.Configuration(configuration);
                }
                else
                {
                    // This is the fallback logger configuration, if there is no Serilog
                    // configuration, this is the default we use
                    loggerBuilder = loggerBuilder
                        .WriteTo.Console(
                            outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message} {NewLine}{Exception}");
                }
            }

            var logger = loggerBuilder
                .Enrich.FromLogContext()
                .Enrich.WithProperty("AppName", AppEnvironment.Name)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.WithProperty("AppVersion", AppEnvironment.Version)
                .CreateLogger();

            Log.Logger = logger;
        }

        public static void FlushLogger() => Log.CloseAndFlush();
    }
}
