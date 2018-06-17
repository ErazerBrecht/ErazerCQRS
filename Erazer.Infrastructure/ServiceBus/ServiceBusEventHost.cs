using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.ServiceBus
{
    public class ServiceBusEventHost : BackgroundService

    {
        private readonly IEventBus _bus;
        private readonly ILogger<ServiceBusEventHost> _logger;

        public ServiceBusEventHost(IEventBus bus, ILogger<ServiceBusEventHost> logger, IApplicationLifetime lifetime,
            IServiceProvider provider)
        {
            _bus = bus;
            _logger = logger;
            lifetime.ApplicationStopping.Register(async () => await _bus.Stop());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.Start(stoppingToken);
            _logger.LogInformation("Listening for events");
        }
    }
}
