using Erazer.Framework.Events.Envelope;
using MediatR;

namespace Erazer.Framework.Events
{
    public interface IEventHandler<T> : INotificationHandler<EventEnvelope<T>> where T: IEvent
    {
    }
}