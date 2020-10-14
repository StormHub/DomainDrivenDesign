using System.Collections.Concurrent;
using System.Collections.Generic;
using Domain.Infrastructure.Messaging;

namespace Domain.Infrastructure
{
    public sealed class DomainEventsCollection
    {
        readonly ConcurrentBag<IDomainEvent> list;

        public DomainEventsCollection()
        {
            list = new ConcurrentBag<IDomainEvent>();
        }

        public void Add<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent
        {
            list.Add(domainEvent);
        }

        public IDomainEvent[] GetAndClear()
        {
            var eventList = new List<IDomainEvent>();
            while (list.TryTake(out IDomainEvent item))
            {
                eventList.Add(item);
            }
            return eventList.ToArray();
        }
    }
}