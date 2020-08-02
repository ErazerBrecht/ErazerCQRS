using System;
using System.Reflection;
using Erazer.Domain.Ticket.Events;
using Erazer.Framework.Cache;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using Erazer.Infrastructure.EventStore;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.Redis;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Write.Application.Commands;
using Erazer.Write.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceStack.Redis;
using SqlStreamStore;

namespace Erazer.Write.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusSettings _busSettings;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _busSettings = configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add 'Configuration'
            services.AddSingleton(_configuration);
            services.Configure<RedisSettings>(_configuration.GetSection("CacheSettings"));

            // Add Telemetry
            services.AddSingletonFactory<ITelemetry, TelemetryFactory>();
            
            // Add 'Infrastructure' Providers
            services.AddSingletonFactory<IStreamStore, EventStoreFactory>();
            services.AddSingletonFactory<IRedisClientsManager, RedisFactory>();
            services.AddBus(x =>
            {
                x.ConnectionString = _busSettings.ConnectionString;
                x.ConnectionName = "Erazer.Write.Web";
                x.UserName = _busSettings.UserName;
                x.Password = _busSettings.Password;
            });
            
            services.AddMediatR
            (
                typeof(CreateTicketCommand).GetTypeInfo().Assembly
            );
            
            services.AddEventStore(_configuration.GetSection("EventStoreSettings"), typeof(TicketCreatedEvent));
            // WITH CACHE
            services.AddScoped<ICache, RedisCache>();
            services.AddScoped<IAggregateRepository>(y => new CacheRepository(new AggregateRepository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            // WITHOUT CACHE
            // services.AddScoped<IAggregateRepository, AggregateRepository>();

            // Application Services
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
