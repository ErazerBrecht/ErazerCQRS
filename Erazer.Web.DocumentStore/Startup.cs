using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MediatR;
using AutoMapper;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Web.Shared.Extensions;
using Erazer.Infrastructure.MongoDb;
using MongoDB.Driver;
using Erazer.Infrastructure.DocumentStore;
using Erazer.Infrastructure.DocumentStore.Repositories;
using EasyNetQ;
using Erazer.Web.Shared.Extensions.DependencyInjection;

namespace Erazer.Web.DocumentStore
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDbSettings"));
            services.Configure<ServiceBusSettings>(_configuration.GetSection("ServiceBusSettings"));

            services.AddSingletonFactory<IMongoDatabase, MongoDbFactory>();
            services.AddSingletonFactory<IBus, BusFactory>();

            services.AddAutoMapper();
            services.AddMediatR();

            services.AddScoped<IFileRepository, FileRepository>();

            //CQRS
            services.StartReciever();

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
                       .WithMethods("GET")
                       .AllowAnyHeader()
                       .SetPreflightMaxAge(TimeSpan.FromHours(1));
            });
            app.UseMvc();
        }
    }
}
