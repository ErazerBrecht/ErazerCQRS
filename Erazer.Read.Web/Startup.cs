using Erazer.Domain.Data.Repositories;
using Erazer.Framework.FrontEnd;
using Erazer.Framework.Events;
using Erazer.Syncing.Infrastructure;

namespace Erazer.Web.ReadAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusSettings _busSettings;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _busSettings = _configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDbSettings"));
            services.Configure<WebsocketSettings>(_configuration.GetSection("WebsocketSettings"));
            services.Configure<EventStoreSettings>(_configuration.GetSection("EventStoreSettings"));

            // Add Telemetry
            services.AddSingletonFactory<ITelemetry, TelemeteryFactory>();

            // Add 'Infrastructure' Providers
            services.AddSingletonFactory<IStreamStore, EventStoreFactory>();
            services.AddSingletonFactory<IMongoDatabase, MongoDbFactory>();
            services.AddScoped<IMongoDbSession, MongoDbSession>();
            services.AddScoped<IWebsocketEmitter, WebsocketEmittor>();
            
            services.AddMongoDbClassMaps();
            services.AddAutoMapper();
            services.AddMediatR();
            services.AddSignalR();

            // TODO Place in separate file (Arne) > services.AddTicket();
            // Query repositories
            services.AddScoped<ITicketQueryRepository, TicketRepository>();
            services.AddScoped<ITicketEventQueryRepository, TicketEventRepository>();
            services.AddScoped<IStatusQueryRepository, StatusRepository>();
            services.AddScoped<IPriorityQueryRepository, PriorityRepository>();

            // Add MVC
            services
                .AddControllers()
                .AddNewtonsoftJson();

            // CQRS
            services.AddSingleton<IEventStore, EventStore>();
            services.AddSingleton<IEventTypeMapping, EventTypeMapping>();
            services.AddSubscriber();
            
            // ServiceBus
            services.AddBus(x =>
            {
                x.ConnectionString = _busSettings.ConnectionString;
                x.UserName = _busSettings.UserName;
                x.Password = _busSettings.Password;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServer server)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200")            // Load this from ENV or Config file
                       .AllowAnyHeader()                                // TODO Temp added for SignalR
                       .AllowCredentials()                              // Added for SignalR
                       .WithMethods("GET", "OPTIONS", "POST")           // OPTIONS & POST are for SignalR 
                       .SetPreflightMaxAge(TimeSpan.FromHours(1));
            });

            app.UseWebsocketEmittor();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
