using Erazer.Framework.Cache;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using Erazer.Web.WriteAPI.Services;

namespace Erazer.Web.WriteAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusSettings _busSettings;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _busSettings = configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add 'Configuration'
            services.AddSingleton(_configuration);
            services.Configure<EventStoreSettings>(_configuration.GetSection("EventStoreSettings"));
            services.Configure<RedisSettings>(_configuration.GetSection("CacheSettings"));

            // Add Telemetry
            services.AddSingletonFactory<ITelemetry, TelemeteryFactory>();
            
            // Add 'Infrastructure' Providers
            services.AddSingletonFactory<IStreamStore, EventStoreFactory>();
            services.AddSingletonFactory<IRedisClientsManager, RedisFactory>();
            services.AddBus(x =>
            {
                x.ConnectionString = _busSettings.ConnectionString;
                x.ConnectionName = "Erazer.Web.WriteAPI";
                x.UserName = _busSettings.UserName;
                x.Password = _busSettings.Password;
            });

            services.AddAutoMapper();
            services.AddMediatR();

            // TODO Place in separate file (Arne)
            services.AddSingleton<IEventStore, EventStore>();
            services.AddSingleton<IEventTypeMapping, EventTypeMapping>();
            // WITH CACHE
            services.AddScoped<ICache, RedisCache>();
            services.AddScoped<IAggregateRepository>(y => new CacheRepository(new AggregateRepository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            // WITHOUT CACHE
            // services.AddScoped<IAggregateRepository, AggregateRepository>();

            services.AddScoped<IFileUploader, FileUploader>();        

            // Add MVC
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200")            // Load this from ENV or Config file
                       .WithMethods("POST", "PUT", "PATCH")
                       .AllowAnyHeader()
                       .SetPreflightMaxAge(TimeSpan.FromHours(1));
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
