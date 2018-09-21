using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using MongoDB.Driver;
using Erazer.Domain.Data.Repositories;
using Erazer.Framework.FrontEnd;
using Erazer.Domain;
using Erazer.Infrastructure.MongoDb;
using Erazer.Infrastructure.EventStore;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.Websockets;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.ReadStore.Repositories;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Web.Shared.Extensions;
using Erazer.Web.Shared.Extensions.DependencyInjection;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit;
using SqlStreamStore;

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

            // Add Singleton TelemeterClient
            services.AddSingletonFactory<TelemetryClient, TelemeteryFactory>();

            // Add 'Infrasructure' Providers
            services.AddSingletonFactory<IStreamStore, EventStoreFactory>();
            services.AddSingletonFactory<IMongoDatabase, MongoDbFactory>();
            services.AddScoped<IMongoDbSession, MongoDbSession>();
            services.AddScoped<IWebsocketEmittor, WebsocketEmittor>();

            services.AddMongoDbClassMaps();
            services.AddAutoMapper();
            services.AddMediatR();
            services.AddSignalR();

            // TODO Place in seperate file (Arne) > services.AddTicket();
            // Query repositories
            services.AddScoped<ITicketQueryRepository, TicketRepository>();
            services.AddScoped<ITicketEventQueryRepository, TicketEventRepository>();
            services.AddScoped<IStatusQueryRepository, StatusRepository>();
            services.AddScoped<IPriorityQueryRepository, PriorityRepository>();

            // Add MVC
            services.AddCors();
            services.AddMvcCore().AddJsonFormatters();

            // CQRS
            services.AddSubscriber();
            services.AddEventBus(x =>
            {
                x.ConnectionString = _busSettings.ConnectionString;
                x.UserName = _busSettings.UserName;
                x.Password = _busSettings.Password;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServer server)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200")            // Load this from ENV or Config file
                       .AllowAnyHeader()                                // TODO Temp added for SignalR
                       .AllowCredentials()                              // Added for SignalR
                       .WithMethods("GET", "OPTIONS", "POST")           // OPTIONS & POST are for SignalR 
                       .SetPreflightMaxAge(TimeSpan.FromHours(1));
            });

            app.UseWebsocketEmittor();
            app.UseMvc();
        }
    }
}
