using Erazer.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Erazer.Framework.Events
{
    public interface IEventStore
    {
        Task Save<T>(Guid aggregateId, int expectedVersion, IEnumerable<IDomainEvent> events,
            CancellationToken cancellationToken = default) where T : AggregateRoot;

        Task<IEnumerable<IDomainEvent>> Get<T>(Guid aggregateId, int fromVersion,
            CancellationToken cancellationToken = default) where T : AggregateRoot;

        IDisposable Subscribe(long? continueAfterPosition,
            Func<long, IDomainEvent, CancellationToken, Task> streamMessageReceived,
            Action<Exception> subscriptionDropped = null);
    }
}