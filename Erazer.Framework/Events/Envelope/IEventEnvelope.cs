using System;
using MediatR;

namespace Erazer.Framework.Events.Envelope
{
    public interface IEventEnvelope<out T>: INotification
    {
        public Guid AggregateRootId { get; }
        public long Created { get; }
        public long Version { get; }
        public long Position { get; }

        public T Event { get; }
    }
}