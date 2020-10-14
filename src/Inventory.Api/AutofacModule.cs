using Autofac;
using Domain.Infrastructure.Messaging;

namespace Inventory.Api
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Command handlers
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)))
                .AsClosedTypesOf(typeof(ICommandHandler<>));

            // Query handlers
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<,>)))
                .AsClosedTypesOf(typeof(IQueryHandler<,>));
        }
    }
}
