using System;

namespace Domain.Common.DateTimes
{
    internal sealed class SystemClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}
