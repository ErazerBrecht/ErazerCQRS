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

        public ServiceBusEventHost(IEventBus bus, ILogger<ServiceBusEventHost> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.Start(stoppingToken);
            _logger.LogInformation("Listening for events");
        }
        
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Stop listing for events");

            _bus?.Stop(cancellationToken);
            return base.StopAsync(cancellationToken);
        }
    }
}
