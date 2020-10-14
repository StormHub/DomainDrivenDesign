using Autofac;
using Domain.Common.DateTimes;
using Serilog;

namespace Domain.Common
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IClock>(
                    c =>
                    {
                        if (c.TryResolve<SystemDateSettings>(out var settings))
                        {
                            Log.Information("SystemDateSettings from config {DateTime}.", settings.SystemDateTime);
                            return new StaticClock(settings.SystemDateTime);
                        }

                        return new SystemClock();
                    }).As<IClock>()
                .SingleInstance();
        }
    }
}
