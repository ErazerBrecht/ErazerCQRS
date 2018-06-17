using System;
using Erazer.Framework.Commands;
using Erazer.Infrastructure.ServiceBus;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit.Commands
{
    public class CommandServiceCollectionConfigurator
    {
        private readonly ServiceCollectionConfigurator _serviceCollectionConfigurator;
        public string CommandQueueName { get; set; }

        public CommandServiceCollectionConfigurator(ServiceCollectionConfigurator serviceCollectionConfigurator)
        {
            _serviceCollectionConfigurator = serviceCollectionConfigurator ?? throw new ArgumentNullException(nameof(serviceCollectionConfigurator));
        }

        public void AddCommandListener<T>() where T : class, ICommand
        {
            _serviceCollectionConfigurator.AddConsumer<CommandConsumer<T>>();
        }
    }
}
