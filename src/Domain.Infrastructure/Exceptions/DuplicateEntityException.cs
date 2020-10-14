using System;
using System.Runtime.Serialization;

namespace Domain.Infrastructure.Exceptions
{
    [Serializable]
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException()
        {
        }

        public DuplicateEntityException(string message)
            : base(message)
        {
        }

        public DuplicateEntityException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public static DuplicateEntityException Of<T>()
        {
            var exception = new DuplicateEntityException();
            exception.WithEntity<T>();
            return exception;
        }

        public static DuplicateEntityException Of<T>(Guid externalId)
        {
            var exception = new DuplicateEntityException();
            exception.WithEntity<T>(externalId);
            return exception;
        }

        public static DuplicateEntityException Of<T>(int internalId)
        {
            var exception = new DuplicateEntityException();
            exception.WithEntity<T>(internalId);
            return exception;
        }

        protected DuplicateEntityException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
