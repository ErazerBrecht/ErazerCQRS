using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Framework.Commands;
using Microsoft.Extensions.Options;

namespace Erazer.Infrastructure.ServiceBus
{
    public class CommandPublisher: ICommandPublisher
    {
        private readonly ICommandBus _bus;

        public CommandPublisher(ICommandBus bus, IOptions<ServiceBusSettings> options)
        {
            _bus = bus;
        }

        public Task Publish<T>(T command, string endpoint) where T : class, ICommand
        {
            return _bus.Send(command, endpoint);
        }

        public Task Publish<T>(IEnumerable<T> commands, string endpoint) where T : class, ICommand
        {
            var tasks = commands.Select(e => _bus.Send(e, endpoint));
            return Task.WhenAll(tasks);
        }
    }
}
