using System.Globalization;
using System.Reflection;

namespace Domain.Common
{
    public static class AppEnvironment
    {
        static AppEnvironment()
        {
            Version = typeof(AppEnvironment).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

            AustraliaCultureInfo = CultureInfo.GetCultureInfo("en-AU");
        }

        public static string Name => "Domain Driven Design";

        public static string Version { get; }

        public static CultureInfo AustraliaCultureInfo { get; }
    }
}
