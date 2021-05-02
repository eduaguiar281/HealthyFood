using System;

namespace HowToDevelop.Core.StoredEvents
{
    public abstract class StoredEventBase<TKey> where TKey : struct
    {
        protected StoredEventBase(in TKey aggregateRootId, in object data, in string eventName, DateTime eventTimeStamp, in string user)
        {
            AggregateRootId = aggregateRootId;
            EventTimeStamp = DateTime.UtcNow;
            Data = data;
            EventName = eventName;
            EventTimeStamp = eventTimeStamp;
            User = user;
        }

        public string Id { get; protected set; }
        public TKey AggregateRootId { get; }
        public object Data { get; }
        public string EventName { get; }
        public DateTime EventTimeStamp { get; }
        public string User { get; }
    }
}
