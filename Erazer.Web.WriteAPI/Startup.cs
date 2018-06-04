﻿using System;
using AutoMapper;
using Erazer.Framework.Cache;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;
using Erazer.Infrastructure.EventStore;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Infrastructure.Redis;
using Erazer.Web.WriteAPI.Services;
using EasyNetQ;
using Erazer.Web.Shared.Extensions.DependencyInjection;

namespace Erazer.Web.WriteAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add 'Configration'
            services.AddSingleton<IConfiguration>(_configuration);
            services.Configure<EventStoreSettings>(_configuration.GetSection("EventStoreSettings"));
            services.Configure<ServiceBusSettings>(_configuration.GetSection("ServiceBusSettings"));
            services.Configure<RedisSettings>(_configuration.GetSection("CacheSettings"));

            // Add 'Infrasructure' Providers
            services.AddSingletonFactory<IEventStoreConnection, EventStoreFactory>();
            services.AddSingletonFactory<IRedisClientsManager, RedisFactory>();
            services.AddSingletonFactory<IBus, BusFactory>();

            services.AddAutoMapper();
            services.AddMediatR();

            // TODO Place in seperate file (Arne)
            services.AddScoped<IEventStore, Infrastructure.EventStore.EventStore> ();
            // WITH CACHE
            services.AddScoped<ICache, RedisCache>();
            services.AddScoped<IAggregateRepository>(y => new CacheRepository(new AggregateRepository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            // WITHOUT CACHE
            //services.AddScoped<IAggregateRepository, AggregateRepository>();

            services.AddScoped<IFileUploader, FileUploader>();

            // CQRS
            services.AddScoped<IEventPublisher, EventPublisher>();

            // Add MVC
            services.AddCors();
            services.AddMvcCore().AddJsonFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200")            // Load this from ENV or Config file
                       .WithMethods("POST", "PUT", "PATCH")
                       .AllowAnyHeader()
                       .SetPreflightMaxAge(TimeSpan.FromHours(1));
            });
            app.UseMvc();
        }
    }
}
