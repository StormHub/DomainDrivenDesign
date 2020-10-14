using System;
using Autofac;
using AutofacSerilogIntegration;
using Domain.Common.Configurations;
using Microsoft.Extensions.Configuration;

namespace Domain.WebApi.Bootstrappers
{
     static class ContainerExtensions
     { 
        internal static void ConfigureContainer(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterLogger(autowireProperties: true);

            var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            if (string.IsNullOrEmpty(databaseSettings?.ConnectionString))
            {
                throw new InvalidOperationException($"{nameof(DatabaseSettings)} is required.");
            }

            builder.RegisterInstance(databaseSettings)
                .AsSelf()
                .SingleInstance();

            Configure(builder);
        }

        static void Configure(ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(
                typeof(AutofacModule).Assembly,
                typeof(Common.AutofacModule).Assembly,
                typeof(Domain.Infrastructure.AutofacModule).Assembly,
                typeof(Data.AutofacModule).Assembly,
                typeof(Orders.Domain.AutofacModule).Assembly,
                typeof(Orders.Api.AutofacModule).Assembly,
                typeof(Inventory.Domain.AutofacModule).Assembly,
                typeof(Inventory.Api.AutofacModule).Assembly);
        }
    }
}
