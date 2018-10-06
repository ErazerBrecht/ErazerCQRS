using System;
using AutoMapper;
using Erazer.Framework.Cache;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;
using Erazer.Infrastructure.EventStore;
using Erazer.Infrastructure.Redis;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Web.WriteAPI.Services;
using Erazer.Web.Shared.Extensions.DependencyInjection;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit;
using SqlStreamStore;


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
            // Add 'Configration'
            services.AddSingleton(_configuration);
            services.Configure<EventStoreSettings>(_configuration.GetSection("EventStoreSettings"));
            services.Configure<RedisSettings>(_configuration.GetSection("CacheSettings"));

            // Add 'Infrasructure' Providers
            services.AddScopedFactory<IStreamStore, EventStoreFactory>();
            services.AddSingletonFactory<IRedisClientsManager, RedisFactory>();
            services.AddCommandBus(x =>
            {
                x.ConnectionString = _busSettings.ConnectionString;
                x.UserName = _busSettings.UserName;
                x.Password = _busSettings.Password;
            });

            services.AddAutoMapper();
            services.AddMediatR();

            // TODO Place in seperate file (Arne)
            services.AddScoped<IEventStore, EventStore> ();
            services.AddSingleton<IEventTypeMapping, EventTypeMapping>();
            // WITH CACHE
            services.AddScoped<ICache, RedisCache>();
            services.AddScoped<IAggregateRepository>(y => new CacheRepository(new AggregateRepository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            // WITHOUT CACHE
            // services.AddScoped<IAggregateRepository, AggregateRepository>();

            services.AddScoped<IFileUploader, FileUploader>();        

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
