using System;
using Erazer.Framework.Commands;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Messages.IntegrationEvents;
using Erazer.Messages.IntegrationEvents.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit
{

    /// <summary>
    /// MassTransit builder Interface
    /// </summary>
    public interface IMassTransitBuilder
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        IServiceCollection Services { get; }
    }

    public class MassTransitBuilder : IMassTransitBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public MassTransitBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public IServiceCollection Services { get; }
    }

    public static class MassTransitBuilderExtensions
    {
        public static IMassTransitBuilder AddEventPublisher(this IMassTransitBuilder builder)
        {
            builder.Services.AddScoped(typeof(IIntegrationEventPublisher<>), typeof(IntigrationEventPublisher<>));
            return builder;
        }

        public static IMassTransitBuilder AddCommandPublisher(this IMassTransitBuilder builder)
        {
            builder.Services.AddScoped(typeof(ICommandPublisher<>), typeof(CommandPublisher<>));
            return builder;
        }

        public static IMassTransitBuilder AddMassTransitEventListerner<T>(this IMassTransitBuilder builder) where T : class, IIntegrationEvent
        {
            builder.Services.AddTransient(typeof(EventReciever<T>), typeof(EventReciever<T>));
            builder.Services.AddSingleton(typeof(IHostedService), typeof(ServiceBusEventHost<T>));
            return builder;
        }

        public static IMassTransitBuilder AddMassTransitCommandListerner<T>(this IMassTransitBuilder builder) where T : class, ICommand
        {
            builder.Services.AddTransient(typeof(CommandReciever<T>), typeof(CommandReciever<T>));
            builder.Services.AddSingleton(typeof(IHostedService), typeof(ServiceBusCommandHost<T>));
            return builder;
        }
    }
}
