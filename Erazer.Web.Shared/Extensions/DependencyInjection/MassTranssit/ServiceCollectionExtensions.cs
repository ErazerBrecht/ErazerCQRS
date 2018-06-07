using Erazer.Infrastructure.ServiceBus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add MassTransit + related stuff to IoC
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="config">The settings for the servicebus</param>
        /// <returns></returns>
        public static IMassTransitBuilder AddMassTransit(this IServiceCollection services, IConfigurationSection config)
        {
            var builder = new MassTransitBuilder(services);

            services.Configure<ServiceBusSettings>(config);

            services.AddSingletonFactory<IBusControl, ServiceBusFactory>();
            services.AddTransient<IServiceBusFactory, ServiceBusFactory>();

            builder.AddEventPublisher();
            builder.AddCommandPublisher();

            return builder;
        }
    }
}
