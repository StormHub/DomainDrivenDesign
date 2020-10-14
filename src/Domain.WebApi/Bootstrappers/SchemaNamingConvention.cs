using System;

namespace Domain.WebApi.Bootstrappers
{
    static class SchemaNamingConvention
    {
        const string Postfix = "dto";

        public static string SelectSchemaId(Type type)
        {
            var prefix = "";
            var namespaces = type.FullName.Split('.');
            if (namespaces.Length > 1)
            {
                prefix = $"{namespaces[1]}.";
            }

            string typeName = type.Name;
            if (typeName.EndsWith(Postfix, StringComparison.InvariantCultureIgnoreCase))
            {
                // Remove the 'dto' from object names
                typeName = typeName.Substring(0, typeName.Length - Postfix.Length);
            }

            return $"{prefix}{typeName}";
        }
    }
}