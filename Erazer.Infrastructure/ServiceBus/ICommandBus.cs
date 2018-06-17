using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Commands;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Erazer.Infrastructure.ServiceBus
{
    public interface ICommandBus: IBus
    {
        Task Send<T>(T message, string reciever) where T : class, ICommand;
    }

    public class CommandBus : ICommandBus
    {
        private readonly IBusControl _bus;

        public CommandBus(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task Send<T>(T message, string reciever) where T : class, ICommand
        {
            var sendToUri = new Uri($"{_bus.Address.Scheme}://{_bus.Address.Host}/{reciever}");
            var endPoint = await _bus.GetSendEndpoint(sendToUri);

            await endPoint.Send(message);
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


    public static class CommandBusFactory
    {
        public static ICommandBus Build(Action<IRabbitMqBusFactoryConfigurator> configure)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(configure);
            return new CommandBus(bus);
        }
    }
}