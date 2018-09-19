using System;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Messages;
using Erazer.Messages.IntegrationEvents.Events;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Jobs
{
    class Program
    {
        private static IConfigurationRoot _configuration;
        private static ServiceBusSettings _busSettings;

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(config =>
                    {
                        config.AddEnvironmentVariables();
                    })
                .ConfigureAppConfiguration((hostContext, config) =>
                    {
                        config.SetBasePath(Environment.CurrentDirectory);
                        config.AddJsonFile("appsettings.json", false);
                        config.AddEnvironmentVariables();

                        _configuration = config.Build();
                        _busSettings = _configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>();
                    })
                .ConfigureLogging((hostContext, config) =>
                {
                    config.AddConsole();
                })
                .ConfigureServices(ConfigureServices);


            await hostBuilder.RunConsoleAsync();
        }


        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddLogging();

            services
                .AddEventBus(x =>
                {
                    x.ConnectionString = _busSettings.ConnectionString;
                    x.UserName = _busSettings.UserName;
                    x.Password = _busSettings.Password;
                })
                .AddEventListeners(x =>
                {
                    x.EventQueueName = EventBusConstants.ErazerJobs;
                    x.AddEventListener<TicketCreatedIntegrationEvent>();
                    x.AddEventListener<TicketPriorityIntegrationEvent>();
                    x.AddEventListener<TicketStatusIntegrationEvent>();
                });

            services.AddMediatR();
        }
    }
}