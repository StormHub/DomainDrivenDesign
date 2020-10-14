using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain.Infrastructure.Exceptions
{
    public static class ExceptionExtensions
    {
        public static Exception WithData(this Exception exception, string key, string value)
        {
            exception.Data[key] = value;
            return exception;
        }

        public static Exception WithEntity<TEntity>(this Exception exception)
        {
            exception.Data[Constants.EntityTypeKey] = typeof(TEntity);
            return exception;
        }

        public static Exception WithEntity<TEntity>(this Exception exception, Guid externalId)
        {
            exception.WithEntity<TEntity>();
            exception.Data[nameof(externalId)] = externalId;
            return exception;
        }

        public static Exception WithEntity<TEntity>(this Exception exception, int internalId)
        {
            exception.WithEntity<TEntity>();
            exception.Data[nameof(internalId)] = internalId;
            return exception;
        }

        public static Exception WithFieldError<T>(this Exception exception, string message, params string[] fieldNames)
        {
            exception.Data[$"{typeof(T).Name}.{string.Join(".", fieldNames)}"] = message;
            return exception;
        }

        public static IDictionary<string, string> GetErrorDetailFields(this Exception exception)
        {
            var fields = new Dictionary<string, string>();

            foreach (DictionaryEntry entry in exception.Data)
            {
                var key = entry.Key.ToString();
                if (key == Constants.EntityTypeKey)
                {
                    var entityType = exception.GetEntityType();
                    if (entityType != null)
                    {
                        fields.Add(Constants.EntityTypeKey, $"{entityType.Name}");
                        continue;
                    }
                }
                if (!(entry.Value is null))
                {
                    fields.Add(entry.Key.ToString(), entry.Value.ToString());
                }
            }

            return fields;
        }

        public static Type GetEntityType(this Exception exception)
        {
            Type entityType = null;
            if (exception.Data.Contains(Constants.EntityTypeKey))
            {
                entityType = exception.Data[Constants.EntityTypeKey] as Type;
            }

            return entityType;
        }
    }
}
