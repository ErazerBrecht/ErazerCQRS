using Erazer.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Framework.Events
{
    public interface IEventStore
    {
        Task Save<T>(Guid aggregateId, int expectedVersion, IEnumerable<IDomainEvent> events) where T : AggregateRoot;
        Task<IEnumerable<IDomainEvent>> Get<T>(Guid aggregateId, int fromVersion) where T : AggregateRoot;
    }
}