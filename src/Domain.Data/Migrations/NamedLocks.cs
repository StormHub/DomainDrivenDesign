using System.Collections.Concurrent;

namespace Domain.Data.Migrations
{
    public class NamedLocks
    {
        // see: http://johnculviner.com/achieving-named-lock-locker-functionality-in-c-4-0/
        // see: https://www.tabsoverspaces.com/233703-named-locks-using-monitor-in-net-implementation

        readonly ConcurrentDictionary<string, object> locks = new ConcurrentDictionary<string, object>();

        public object GetLock(string key)
        {
            return locks.GetOrAdd(key, _ => new object());
        }
    }
}