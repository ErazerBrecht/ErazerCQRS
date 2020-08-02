using System;

namespace Erazer.Framework.Events.Envelope
{
    public class EventEnvelope<T>: IEventEnvelope<T>
    {
        public Guid AggregateRootId { get; }
        public long Created { get; }
        public long Version { get; }
        public long Position { get; }

        public T Event { get; }

        public EventEnvelope(Guid aggregateRootId, long created, long version, long position, T @event)
        {
            AggregateRootId = aggregateRootId;
            Created = created;
            Version = version;
            Position = position;
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
        }
    }
}