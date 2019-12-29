using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Infrastructure.ServiceBus.Commands;
using Erazer.Infrastructure.ServiceBus.Events;
using Erazer.Messages.Commands.Infrastructure;
using Erazer.Messages.IntegrationEvents.Infrastructure;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add 'Bus' related stuff to IoC
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="configure">Action to be able to configure the bus</param>
        /// <returns></returns>
        public static void AddBus(this IServiceCollection services, Action<ServiceBusSettings> configure)
        {
            var config = new ServiceBusSettings();
            configure?.Invoke(config);
            
            services.AddScoped<IIntegrationEventPublisher, EventPublisher>();
            services.AddScoped<ICommandPublisher, CommandPublisher>();
            services.TryAddSingleton<ITelemetry, TelemetryLogging>();
            
            services.AddMassTransit(ConfigureBus(config), cfg => { cfg.AddConsumers(config); });
        }

        private static Func<IServiceProvider, IBusControl> ConfigureBus(ServiceBusSettings settings)
        {
            IBusControl Factory(IServiceProvider provider)
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(settings.ConnectionString), settings.ConnectionName, h =>
                    {
                        h.Username(settings.UserName);
                        h.Password(settings.Password);
                    });

                    cfg.AddEndpoints(provider, settings);
                });

                return busControl;
            }

            return Factory;
        }
    }
}