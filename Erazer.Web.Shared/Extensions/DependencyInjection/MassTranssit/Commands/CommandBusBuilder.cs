using System;
using Erazer.Framework.Commands;
using Erazer.Infrastructure.ServiceBus;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit.Commands
{

    /// <summary>
    /// CommandBusBuilder builder Interface
    /// </summary>
    public interface ICommandBusBuilder
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        IServiceCollection Services { get; }
        /// <summary>
        /// TODO
        /// </summary>
        ServiceBusSettings Settings { get; }
    }

    public class CommandBusBuilder : ICommandBusBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBusBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="settings">TODO</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public CommandBusBuilder(IServiceCollection services, ServiceBusSettings settings)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Settings = settings;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public IServiceCollection Services { get; }

        public ServiceBusSettings Settings { get; }
    }

    public static class CommandBusBuilderExtensions
    {
        public static ICommandBusBuilder AddCommandPublisher(this ICommandBusBuilder builder)
        {
            builder.Services.AddScoped<ICommandPublisher, CommandPublisher>();
            return builder;
        }

        public static ICommandBusBuilder AddCommandListeners(this ICommandBusBuilder builder, Action<CommandServiceCollectionConfigurator> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            var configurator = new ServiceCollectionConfigurator(builder.Services);
            var wrapper = new CommandServiceCollectionConfigurator(configurator);

            configure(wrapper);

            builder.Services.AddSingleton(configurator);
            builder.Services.AddSingleton(provider => CommandBusFactory.Build(cfg =>
            {
                var host = cfg.Host(new Uri(builder.Settings.ConnectionString), h =>
                {
                    h.Username(builder.Settings.UserName);
                    h.Password(builder.Settings.Password);
                });

                cfg.ReceiveEndpoint(host, wrapper.CommandQueueName, e =>
                {
                    e.LoadFrom(provider);
                });
            }));

            builder.Services.AddSingleton<IHostedService, ServiceBusCommandHost>();
            return builder;
        }
    }
}
