using Autofac;
using Domain.Infrastructure.Messaging;

namespace Inventory.Domain
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IDomainEventHandler<>)))
                .AsClosedTypesOf(typeof(IDomainEventHandler<>));
        }
    }
}
