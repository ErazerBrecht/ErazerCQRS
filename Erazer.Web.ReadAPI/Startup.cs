using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using MongoDB.Driver;
using Erazer.Servicebus;
using Erazer.Servicebus.Extensions;
using Erazer.Shared.Extensions.DependencyInjection;
using Erazer.Web.ReadAPI.Extensions;
using Erazer.Web.Shared.Telemetery;
using Erazer.DAL.ReadModel.Repositories;
using Erazer.Domain.Infrastructure.Repositories;
using Erazer.Framework.FrontEnd;
using Erazer.Web.Read.API.Websockets;
using Erazer.DAL.Infrastucture.MongoDb;
using Erazer.Domain;
using Erazer.DAL.Infrastucture.EventStore;
using EventStore.ClientAPI;

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
            services.Configure<WebsocketSettings>(_configuration.GetSection("WebsocketSettings"));
            services.Configure<EventStoreSettings>(_configuration.GetSection("EventStoreSettings"));

            // Add Singleton TelemeterClient
            services.AddSingletonFactory<TelemetryClient, TelemeteryFactory>();

            // Add 'Infrasructure' Providers
            services.AddSingletonFactory<IMongoDatabase, MongoDbFactory>();
            services.AddSingleton<IWebsocketEmittor, WebsocketEmittor>();
            services.AddSingletonFactory<IEventStoreConnection, EventStoreFactory>();

            services.AddAutoMapper();
            services.AddMediatR();

            // TODO Place in seperate file (Arne) > services.AddTicket();
            // Query repositories
            services.AddScoped<ITicketQueryRepository, TicketRepository>();
            services.AddScoped<ITicketEventQueryRepository, TicketEventRepository>();
            services.AddScoped<IStatusQueryRepository, StatusRepository>();
            services.AddScoped<IPriorityQueryRepository, PriorityRepository>();

            // CQRS
            services.StartSubscriber<Ticket>();

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
