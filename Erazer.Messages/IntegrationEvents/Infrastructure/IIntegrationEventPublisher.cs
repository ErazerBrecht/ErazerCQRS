using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Messages.IntegrationEvents.Infrastructure
{
    public interface IIntegrationEventPublisher
    {
        Task Publish<T>(T @event) where T : class, IIntegrationEvent;
        Task Publish<T>(IEnumerable<T> events) where T : class, IIntegrationEvent;
    }
}
