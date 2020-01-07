using System;
using System.Reflection;
using System.Threading.Tasks;
using Erazer.Domain.Events;
using Erazer.Infrastructure.ReadStore;
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

        public static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Environment.CurrentDirectory);
                    config.AddJsonFile("appsettings.json", false);
                    config.AddEnvironmentVariables();

                    _configuration = config.Build();
                })
                .ConfigureLogging((hostContext, config) => { config.AddConsole(); })
                .ConfigureServices(ConfigureServices);


            await hostBuilder.RunConsoleAsync();
        }


        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddLogging();

            services
                .AddMongo(_configuration.GetSection("MongoDbSettings"), DbCollectionsSetup.ReadStoreConfiguration)
                .AddEventStore(_configuration.GetSection("EventStoreSettings"))
                .AddSubscriber();
            
            services.AddMediatR(typeof(TicketCommentDomainEvent).GetTypeInfo().Assembly);
        }
    }
}