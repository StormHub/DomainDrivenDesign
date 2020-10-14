using System;
using System.Runtime.Serialization;

// ReSharper disable UnusedMember.Local
namespace Domain.Infrastructure.Exceptions
{
    [Serializable]
    public sealed class DomainDataRuleException : DomainRuleException
    {
        DomainDataRuleException()
        {
        }

        DomainDataRuleException(string message) : base(message)
        {
        }

        DomainDataRuleException(string errorCode, string message)
            : base(errorCode, message)
        {
        }

        DomainDataRuleException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        DomainDataRuleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static DomainDataRuleException WithCode<T>(string code, string message)
        {
            var exception = new DomainDataRuleException(code, message);
            exception.WithEntity<T>();
            return exception;
        }

        public static DomainDataRuleException Of<T>(string message, params string[] fieldNames)
        {
            var exception = new DomainDataRuleException(message);
            exception.WithEntity<T>();
            if (fieldNames != null)
            {
                exception.WithData($"{typeof(T).Name}.{string.Join(".", fieldNames)}", message);
            }
            return exception;
        }

        public static DomainDataRuleException Required<T>(params string[] fieldNames)
        {
            var exception = new DomainDataRuleException();
            exception.WithEntity<T>();
            if (fieldNames != null)
            {
                exception.WithData($"{typeof(T).Name}.{string.Join(".", fieldNames)}", nameof(Required));
            }
            return exception;
        }
    }
}
