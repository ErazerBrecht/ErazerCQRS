using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Erazer.Messages.IntegrationEvents.Infrastructure
{
    public interface IIntegrationEventPublisher
    {
        Task Publish<T>(T @event, CancellationToken cancellationToken = default) where T : class, IIntegrationEvent;
        Task Publish<T>(IEnumerable<T> events, CancellationToken cancellationToken = default) where T : class, IIntegrationEvent;
    }
}
