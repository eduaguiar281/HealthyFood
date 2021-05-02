using System;

namespace HowToDevelop.Core.StoredEvents
{
    public class StoredEvent : StoredEventBase<int>
    {
        public StoredEvent(int aggregateRootId, object data, string eventName, DateTime eventTimeStamp, string user)
            :base(aggregateRootId, data, eventName, eventTimeStamp, user)
        {

        }
    }
}
