using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Messages.IntegrationEvents;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Erazer.Infrastructure.ServiceBus
{
    public interface IEventBus: IBus
    {
        Task Publish<T>(T message) where T : class, IIntegrationEvent;
    }

    public class EventBus : IEventBus
    {
        private readonly IBusControl _bus;

        public EventBus(IBusControl bus)
        {
            _bus = bus;
        }

        public Task Publish<T>(T message) where T : class, IIntegrationEvent
        {
            return _bus.Publish(message);
        }

        public Task Start(CancellationToken cancellationToken)
        {
            return _bus.StartAsync(cancellationToken);
        }

        public Task Stop(CancellationToken cancellationToken)
        {
            return _bus.StopAsync(cancellationToken);
        }
    }


    public static class EventBusFactory
    {
        public static IEventBus Build(Action<IRabbitMqBusFactoryConfigurator> configure)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(configure);
            return new EventBus(bus);
        }
    }
}