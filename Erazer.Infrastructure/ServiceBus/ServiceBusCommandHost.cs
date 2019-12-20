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

        public ServiceBusCommandHost(ICommandBus bus, ILogger<ServiceBusCommandHost> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.Start(stoppingToken);
            _logger.LogInformation("Listening for commands");
        }
        
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Stop listing for commands");

            _bus?.Stop(cancellationToken);
            return base.StopAsync(cancellationToken);
        }
    }
}
