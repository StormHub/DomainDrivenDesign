using Autofac;
using Domain.Data.Migrations;
using Domain.Data.Repository;
using Domain.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlAppLockDatabaseLock>()
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<DbUpgrader>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<DbUpgradeManager>()
                .SingleInstance();

            builder.RegisterType<DbContextOptionsInitializer>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<DbContextOptionsInitializer>().Create<AppDbContext>())
                .As<DbContextOptions<AppDbContext>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AppDbContext>()
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType(typeof(EFCoreUnitOfWork<AppDbContext>))
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(AppDbRepository<>))
                .As(typeof(IRepository<>));
        }
    }
}
