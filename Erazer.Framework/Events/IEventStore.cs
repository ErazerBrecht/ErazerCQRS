using Erazer.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Events.Envelope;

namespace Erazer.Framework.Events
{
    public interface IEventStore
    {
        Task Save<T>(Guid aggregateId, int expectedVersion, IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default) where T : AggregateRoot;

        Task<IEnumerable<IEventEnvelope<IEvent>>> Get<T>(Guid aggregateId, int fromVersion,
            CancellationToken cancellationToken = default) where T : AggregateRoot;

        Task<(bool IsEnd, IEnumerable<IEventEnvelope<IEvent>> EventEnvelopes)> GetAll(long fromPosition, int pageSize,
            CancellationToken cancellationToken = default);

        IDisposable Subscribe(long? continueAfterPosition,
            Func<IEventEnvelope<IEvent>, CancellationToken, Task> streamMessageReceived,
            Action<Exception> subscriptionDropped = null, Action<bool> hasCaughtUp = null, int? pageSize = null);
    }
}