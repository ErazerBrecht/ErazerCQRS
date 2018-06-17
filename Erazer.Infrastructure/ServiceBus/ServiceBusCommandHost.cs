using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.ServiceBus
{
    public class ServiceBusCommandHost: BackgroundService
    {
        private readonly ICommandBus _bus;
        private readonly ILogger<ServiceBusCommandHost> _logger;

        public ServiceBusCommandHost(ICommandBus bus, ILogger<ServiceBusCommandHost> logger, IApplicationLifetime lifetime, IServiceProvider provider)
        {
            _bus = bus;
            _logger = logger;
            lifetime.ApplicationStopping.Register(async () => await _bus.Stop());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.Start(stoppingToken);
            _logger.LogInformation("Listening for commands");
        }
    }
}
