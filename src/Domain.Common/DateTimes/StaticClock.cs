using System;

namespace Domain.Common.DateTimes
{
    internal sealed class StaticClock : IClock
    {
        readonly DateTimeOffset systemStartTime;
        readonly long startTicks;

        public StaticClock(DateTimeOffset systemStartTime)
        {
            this.systemStartTime = systemStartTime;
            startTicks = DateTime.Now.Ticks;
        }

        public DateTimeOffset UtcNow => systemStartTime.Add(Duration);

        public DateTimeOffset Now => systemStartTime.Add(Duration);

        TimeSpan Duration => TimeSpan.FromTicks(DateTime.Now.Ticks - startTicks);
    }
}
