using System;

namespace Domain.Common.DateTimes
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }

        DateTimeOffset Now { get; }
    }
}
