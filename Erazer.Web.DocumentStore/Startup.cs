using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MediatR;
using AutoMapper;
using Erazer.Infrastructure.MongoDb;
using MongoDB.Driver;
using Erazer.Infrastructure.DocumentStore;
using Erazer.Infrastructure.DocumentStore.Repositories;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Messages;
using Erazer.Messages.Commands;
using Erazer.Web.Shared.Extensions.DependencyInjection;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit.Commands;

namespace Erazer.Web.DocumentStore
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
            services.AddSingleton(_configuration);
            services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDbSettings"));

            services.AddSingletonFactory<IMongoDatabase, MongoDbFactory>();

            services.AddAutoMapper();
            services.AddMediatR();

            services.AddScoped<IFileRepository, FileRepository>();


            // Add MVC
            services.AddCors();
            services.AddMvcCore().AddJsonFormatters();

            services
                .AddCommandBus(x =>
                {
                    x.ConnectionString = _busSettings.ConnectionString;
                    x.UserName = _busSettings.UserName;
                    x.Password = _busSettings.Password;
                })
                .AddCommandListeners(x =>
                {
                    x.CommandQueueName = CommandBusConstants.ErazerDocumentStore;
                    x.AddCommandListener<UploadFileCommand>();
                });
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
