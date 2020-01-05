using System;
using System.Reflection;
using AutoMapper;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.ReadStore;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Infrastructure.Websockets;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

            // Add Telemetry
            services.AddSingletonFactory<ITelemetry, TelemetryFactory>();

            // Add 'Infrastructure' Providers
            services.AddWebsocketEmitter(_configuration.GetSection("WebsocketSettings"));
            services.AddMongo(_configuration.GetSection("MongoDbSettings"), DbCollectionsSetup.ReadStoreConfiguration);

            // Add 'Application services'
            services.AddAutoMapper(typeof(Startup).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            // Add MVC
            services
                .AddControllers()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory,
            IServer server)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200") // Load this from ENV or Config file
                    .AllowAnyHeader() // TODO Temp added for SignalR
                    .AllowCredentials() // Added for SignalR
                    .WithMethods("GET", "OPTIONS", "POST") // OPTIONS & POST are for SignalR 
                    .SetPreflightMaxAge(TimeSpan.FromHours(1));
            });

            app.UseWebsocketEmitter();
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}