using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using MassTransit;

namespace Erazer.Infrastructure.ServiceBus
{
    public class EventPublisher<T>: IEventPublisher<T> where T : class, IEvent
    {
        private readonly IBusControl _bus;

        public EventPublisher(IBusControl bus)
        {
            _bus = bus;
        }

        public Task Publish(T @event)
        {
            return _bus.Publish(@event);
        }

        public Task Publish(IEnumerable<T> events)
        {
            var tasks = events.Select(e => _bus.Publish(e));
            return Task.WhenAll(tasks);
        }
    }
}
