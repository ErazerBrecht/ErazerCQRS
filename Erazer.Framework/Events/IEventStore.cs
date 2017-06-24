using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Framework.Events
{
    public interface IEventStore
    {
        Task Save(Guid aggregateId, IEnumerable<IEvent> events);
        Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion);
    }
}