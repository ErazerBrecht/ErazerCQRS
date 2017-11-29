using System;
using AutoMapper;
using Erazer.DAL.ReadModel.Base;
using Erazer.DAL.ReadModel.Repositories;
using Erazer.Servicebus;
using Erazer.Servicebus.Extensions;
using Erazer.Services.Events;
using Erazer.Services.Queries.Repositories;
using Erazer.Shared.Extensions.DependencyInjection;
using Erazer.Web.ReadAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Erazer.Websockets;
using Microsoft.ApplicationInsights;
using Erazer.Web.Shared.Telemetery;

namespace Erazer.Web.ReadAPI
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(_configuration);
            services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDbSettings"));
            services.Configure<AzureServiceBusSettings>(_configuration.GetSection("AzureServiceBusSettings"));
            services.Configure<WebsocketSettings>(_configuration.GetSection("WebsocketSettings"));

            // Add Singleton TelemeterClient
            services.AddSingletonFactory<TelemetryClient, TelemeteryFactory>();

            // Add 'Infrasructure' Providers
            services.AddSingletonFactory<IMongoDatabase, MongoDbFactory>();
            services.AddSingletonFactory<IQueueClient, QueueClientFactory>();
            services.AddSingleton<IWebsocketEmittor, WebsocketEmittor>();

            services.AddAutoMapper();
            services.AddMediatR();

            // TODO Place in seperate file (Arne)
            // Query repositories
            services.AddScoped<ITicketQueryRepository, TicketRepository>();
            services.AddScoped<ITicketEventQueryRepository, TicketEventRepository>();
            services.AddScoped<IStatusQueryRepository, StatusRepository>();
            services.AddScoped<IPriorityQueryRepository, PriorityRepository>();

            // CQRS
            services.StartEventReciever();

            // Add MVC
            services.AddCors();
            services.AddMvcCore().AddJsonFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServer server)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
            }

            app.UseMongoDbClassMaps();

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200")            // Load this from ENV or Config file
                       .WithMethods("GET")
                       .SetPreflightMaxAge(TimeSpan.FromHours(1));
            });

            app.UseMvc();
            app.Seed();
        }
    }
}
