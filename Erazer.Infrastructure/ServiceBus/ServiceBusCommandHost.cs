using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Commands;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.ServiceBus
{
    public class ServiceBusCommandHost<T>: BackgroundService where T : class, ICommand
    {
        private readonly IBusControl _bus;
        private readonly ILogger<ServiceBusCommandHost<T>> _logger;

        public ServiceBusCommandHost(IServiceBusFactory factory, ILogger<ServiceBusCommandHost<T>> logger, IApplicationLifetime lifetime, IServiceProvider provider)
        {
            _logger = logger;

            _bus = factory.Configure((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, $"ErazerCommandQueue-{typeof(T).Name}", e =>
                {
                    e.Consumer(typeof(CommandReciever<T>), provider.GetService);
                });
            });

            lifetime.ApplicationStopping.Register(async () => await _bus.StopAsync());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.StartAsync(stoppingToken);
            _logger.LogInformation("Listening for commands");
        }
    }
}
