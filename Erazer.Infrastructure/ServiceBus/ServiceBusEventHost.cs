using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Messages.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.ServiceBus
{
    public class ServiceBusEventHost<T> : BackgroundService where T : class, IIntegrationEvent

    {
        private readonly IBusControl _bus;
        private readonly ILogger<ServiceBusEventHost<T>> _logger;

        public ServiceBusEventHost(IServiceBusFactory factory, ILogger<ServiceBusEventHost<T>> logger, IApplicationLifetime lifetime,
            IServiceProvider provider)
        {
            _logger = logger;

            _bus = factory.Configure((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, "ErazerEventQueue",
                    e => { e.Consumer(typeof(EventReciever<T>), provider.GetService); });
            });

            lifetime.ApplicationStopping.Register(async () => await _bus.StopAsync());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.StartAsync(stoppingToken);
            _logger.LogInformation("Listening for events");
        }
    }
}
