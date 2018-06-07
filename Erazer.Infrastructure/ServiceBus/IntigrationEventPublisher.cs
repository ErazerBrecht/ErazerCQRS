using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Messages.IntegrationEvents;
using Erazer.Messages.IntegrationEvents.Infrastructure;
using MassTransit;

namespace Erazer.Infrastructure.ServiceBus
{
    public class IntigrationEventPublisher<T>: IIntegrationEventPublisher<T> where T : class, IIntegrationEvent
    {
        private readonly IBusControl _bus;

        public IntigrationEventPublisher(IBusControl bus)
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
