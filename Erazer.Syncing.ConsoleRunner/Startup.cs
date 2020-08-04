using System;
using System.Reflection;
using AutoMapper;
using Erazer.Domain.Ticket.Events;
using Erazer.Infrastructure.ReadStore;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Infrastructure.Websockets;
using Erazer.Read.Mapping;
using Erazer.Syncing.Handlers.TicketSyncHandlers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Erazer.Syncing.ConsoleRunner
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusSettings _busSettings;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _busSettings = _configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddCors();
            services.AddWebsocketEmitter();
            
            // Add Eventstore + subscriber
            services
                .AddMongo(_configuration.GetSection("MongoDbSettings"), DbCollectionsSetup.ReadStoreConfiguration)
                .AddEventStore(_configuration.GetSection("EventStoreSettings"), typeof(TicketCreatedEvent))
                .AddComboSubscriber();
            //.AddLiveSubscriber();
            //.AddReSyncSubscriber();

            // Add ServiceBus
            services.AddBus(x =>
            {
                x.ConnectionString = _busSettings.ConnectionString;
                x.ConnectionName = "Erazer.Syncing.ConsoleRunner";
                x.UserName = _busSettings.UserName;
                x.Password = _busSettings.Password;
            });
            
            services.AddAutoMapper(typeof(TicketMappings).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(TicketCreateEventHandler).GetTypeInfo().Assembly);
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
                builder.WithOrigins("http://localhost:4200") // Load this from ENV or Config file
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromHours(1));
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ReduxEventHub>("/events");
            });
        }
    }
}