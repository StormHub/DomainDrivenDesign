using Autofac;
using Domain.WebApi.Filters.ExceptionHandlers;

namespace Domain.WebApi
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpErrorBuilder>()
                .As<IHttpErrorBuilder>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainRuleExceptionHandler>()
                .As<IExceptionHandler>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EntityNotFoundExceptionHandler>()
                .As<IExceptionHandler>()
                .InstancePerLifetimeScope();
        }
    }
}
