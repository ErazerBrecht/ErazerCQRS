using AutoMapper;
using Erazer.DAL.WriteModel;
using Erazer.Domain;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using Erazer.Framework.Factories;
using Erazer.Servicebus;
using Erazer.Shared.Extensions.DependencyInjection;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Erazer.Web.WriteAPI
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
            // Add 'Configration'
            services.AddSingleton<IConfiguration>(_configuration);
            services.Configure<EventStoreSettings>(_configuration.GetSection("EventStoreSettings"));
            services.Configure<AzureServiceBusSettings>(_configuration.GetSection("AzureServiceBusSettings"));

            // Add 'Infrasructure' Providers
            services.AddSingletonFactory<IEventStoreConnection, EventStoreFactory>();
            services.AddSingletonFactory<IQueueClient, QueueClientFactory>();

            services.AddAutoMapper();
            services.AddMediatR();

            // TODO Place in seperate file (Arne)
            services.AddSingleton<IFactory<Ticket>, AggregateFactory<Ticket>>();
            services.AddScoped<IAggregateRepository<Ticket>, AggregateRepository<Ticket>>();
            services.AddScoped<IEventStore, DAL.WriteModel.EventStore>();

            // CQRS
            services.AddScoped<IEventPublisher, EventPublisher>();

            // Add MVC
            services.AddMvcCore().AddJsonFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
