using System;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Messages.IntegrationEvents;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit.Events
{
    public class EventServiceCollectionConfigurator
    {
        private readonly ServiceCollectionConfigurator _serviceCollectionConfigurator;
        public string EventQueueName { get; set; }

        public EventServiceCollectionConfigurator(ServiceCollectionConfigurator serviceCollectionConfigurator)
        {
            _serviceCollectionConfigurator = serviceCollectionConfigurator ?? throw new ArgumentNullException(nameof(serviceCollectionConfigurator));
        }

        public void AddEventListener<T>() where T : class, IIntegrationEvent
        {
            _serviceCollectionConfigurator.AddConsumer<EventConsumer<T>>();
        }
    }
}
