using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Messages.IntegrationEvents.Infrastructure
{
    public interface IIntegrationEventPublisher<T> where T : class, IIntegrationEvent
    {
        Task Publish(T @event);
        Task Publish(IEnumerable<T> events);
    }
}
