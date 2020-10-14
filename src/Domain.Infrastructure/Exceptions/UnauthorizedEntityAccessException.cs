using System;
using System.Runtime.Serialization;

namespace Domain.Infrastructure.Exceptions
{
    [Serializable]
    public class UnauthorizedEntityAccessException : Exception
    {
        public UnauthorizedEntityAccessException() : this(null)
        {
        }

        public UnauthorizedEntityAccessException(string message)
            : this(message, null)
        {
        }

        public UnauthorizedEntityAccessException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected UnauthorizedEntityAccessException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public static UnauthorizedEntityAccessException Of<T>()
        {
            var exception = new UnauthorizedEntityAccessException();
            exception.WithEntity<T>();
            return exception;
        }

        public static UnauthorizedEntityAccessException Of<T>(Guid externalId)
        {
            var exception = new UnauthorizedEntityAccessException();
            exception.WithEntity<T>(externalId);
            return exception;
        }
    }
}