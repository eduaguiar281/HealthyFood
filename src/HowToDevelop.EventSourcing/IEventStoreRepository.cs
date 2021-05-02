using HowToDevelop.Core.StoredEvents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HowToDevelop.EventSourcing
{
    public interface IEventStoreRepository
    {
        Task SaveAsync(StoredEvent @event);
        Task<IEnumerable<StoredEvent>> GetByIdAsync(int aggregateRootId);
    }
}
