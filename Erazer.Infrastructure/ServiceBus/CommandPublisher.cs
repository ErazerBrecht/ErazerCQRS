using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Framework.Commands;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Erazer.Infrastructure.ServiceBus
{
    public class CommandPublisher<T> : ICommandPublisher<T> where T : class, ICommand
    {
        private readonly IBusControl _bus;
        private readonly Uri _sendUri;

        public CommandPublisher(IBusControl bus, IOptions<ServiceBusSettings> options)
        {
            _bus = bus;
            _sendUri = new Uri($"{options.Value.ConnectionString}" + "ErazerCommandQueue");
        }

        public async Task Publish(T command)
        {
            var endPoint = await _bus.GetSendEndpoint(_sendUri);
            await endPoint.Send(command);
        }

        public async Task Publish(IEnumerable<T> commands)
        {
            var endPoint = await _bus.GetSendEndpoint(_sendUri);

            var tasks = commands.Select(e => endPoint.Send(e));
            await Task.WhenAll(tasks);
        }
    }
}
