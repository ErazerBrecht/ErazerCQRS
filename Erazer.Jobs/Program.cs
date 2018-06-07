using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Erazer.Messages.IntegrationEvents.Events;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Jobs
{
    class Program
    {
        private static IConfigurationRoot _configuration;

        public static async Task Main(string[] args)
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

            
            services.AddMassTransit(_configuration.GetSection("ServiceBusSettings"))
                .AddMassTransitEventListerner<TicketCreatedIntegrationEvent>()
                .AddMassTransitEventListerner<TicketPriorityIntegrationEvent>();

            services.AddMediatR();
        }
    }
}
