using System;
using System.Runtime.Serialization;

namespace Domain.Infrastructure.Exceptions
{
    [Serializable]
    public class DomainRuleException : Exception
    {
        [NonSerialized]
        readonly string errorCode;

        public DomainRuleException()
        {
            errorCode = null;
        }

        public DomainRuleException(string errorCode, string message)
            : base(message)
        {
            this.errorCode = errorCode;
        }

        public DomainRuleException(string message)
            : base(message)
        {
        }

        public DomainRuleException(string errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.errorCode = errorCode;
        }

        public DomainRuleException(string message, Exception innerException)
            : base(message, innerException)
        {
            errorCode = null;
        }

        public string GetErrorCode() => errorCode;

        protected DomainRuleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
