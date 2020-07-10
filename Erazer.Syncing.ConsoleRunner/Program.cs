using System;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Events;
using Erazer.Infrastructure.ReadStore;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Infrastructure.Websockets;
using Erazer.Read.Mapping;
using Erazer.Syncing.Handlers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Syncing.ConsoleRunner
{
    class Program
    {
        private static IConfigurationRoot _configuration;
        private static ServiceBusSettings _busSettings;
        

        public static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Environment.CurrentDirectory);
                    config.AddJsonFile("appsettings.json", false);
                    config.AddEnvironmentVariables();

                    _configuration = config.Build();
                    _busSettings = _configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>();
                })
                .ConfigureLogging((hostContext, config) => { config.AddConsole(); })
                .ConfigureServices(ConfigureServices);


            await hostBuilder.RunConsoleAsync();
        }


        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddLogging();

            // Add Eventstore + subscriber
            services
                .AddMongo(_configuration.GetSection("MongoDbSettings"), DbCollectionsSetup.ReadStoreConfiguration)
                .AddEventStore(_configuration.GetSection("EventStoreSettings"), typeof(TicketCreateDomainEvent))
                //.AddLiveSubscriber();
                .AddReSyncSubscriber();
            
            // Add ServiceBus
            services.AddBus(x =>
            {
                x.ConnectionString = _busSettings.ConnectionString;
                x.ConnectionName = "Erazer.Syncing.ConsoleRunner";
                x.UserName = _busSettings.UserName;
                x.Password = _busSettings.Password;
            });

            services.AddWebsocketEmitter();
            services.AddAutoMapper(typeof(TicketMappings).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(TicketCreateEventHandler).GetTypeInfo().Assembly);
        }
    }
}