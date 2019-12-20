using System;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit.Commands;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit.Events;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add EventBus related stuff to IoC
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="configureSettings">Action to be able to configure the event bus</param>
        /// <returns></returns>
        public static IEventBusBuilder AddEventBus(this IServiceCollection services, Action<ServiceBusSettings> configureSettings = null)
        {
            var settings = new ServiceBusSettings();
            configureSettings?.Invoke(settings);

            var builder = new EventBusBuilder(services, settings);

            services.AddSingleton(settings);
            services.AddSingleton(provider => EventBusFactory.Build(cfg =>
            {
                cfg.Host(new Uri(settings.ConnectionString), h =>
                {
                    h.Username(settings.UserName); 
                    h.Password(settings.Password);
                });
            }));

            builder.AddEventPublisher();
            return builder;
        }

        /// <summary>
        /// Add CommandBus related stuff to IoC
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="configureSettings">Action to be able to configure the command bus</param>
        /// <returns></returns>
        public static ICommandBusBuilder AddCommandBus(this IServiceCollection services, Action<ServiceBusSettings> configureSettings = null)
        {
            var settings = new ServiceBusSettings();
            configureSettings?.Invoke(settings);

            var builder = new CommandBusBuilder(services, settings);

            services.AddSingleton(settings);
            services.AddSingleton(provider => CommandBusFactory.Build(cfg =>
            {
                cfg.Host(new Uri(settings.ConnectionString), h =>
                {
                    h.Username(settings.UserName);
                    h.Password(settings.Password);
                });
            }));

            builder.AddCommandPublisher();
            return builder;
        }
    }
}
