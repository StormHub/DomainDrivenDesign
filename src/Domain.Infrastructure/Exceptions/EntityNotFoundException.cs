using System;
using System.Runtime.Serialization;

namespace Domain.Infrastructure.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public static EntityNotFoundException Of<T>() 
        {
            var exception = new EntityNotFoundException();
            exception.WithEntity<T>();
            return exception;
        }

        public static EntityNotFoundException Of<T>(Guid externalId)
        {
            var exception = new EntityNotFoundException();
            exception.WithEntity<T>(externalId);
            return exception;
        }

        public static EntityNotFoundException Of<T>(int internalId)
        {
            var exception = new EntityNotFoundException();
            exception.WithEntity<T>(internalId);
            return exception;
        }

        protected EntityNotFoundException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
