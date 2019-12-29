using Erazer.Messages.IntegrationEvents.Models;

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
                .ConfigureHostConfiguration(config => { config.AddEnvironmentVariables(); })
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

            services.AddBus(x =>
            {
                x.ConnectionString = _busSettings.ConnectionString;
                x.UserName = _busSettings.UserName;
                x.Password = _busSettings.Password;
                x.ConnectionName = "Erazer.Web.Jobs";
                x.AddEvents(y =>
                {
                    y.QueueName = "EventQueue.Erazer.Jobs";
                    y.AddEventListener<TicketCreatedIntegrationEvent>();
                    y.AddEventListener<TicketPriorityIntegrationEvent>();
                    y.AddEventListener<TicketStatusIntegrationEvent>();
                });
            });

            services.AddMediatR();
        }
    }
}