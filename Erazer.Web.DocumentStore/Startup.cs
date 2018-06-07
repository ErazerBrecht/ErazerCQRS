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
using Erazer.Messages.Commands;
using Erazer.Web.Shared.Extensions.DependencyInjection;
using Erazer.Web.Shared.Extensions.DependencyInjection.MassTranssit;

namespace Erazer.Web.DocumentStore
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
            services.AddSingleton(_configuration);
            services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDbSettings"));

            services.AddSingletonFactory<IMongoDatabase, MongoDbFactory>();
            services.AddMassTransit(_configuration.GetSection("ServiceBusSettings"))
                .AddMassTransitCommandListerner<UploadFileCommand>();

            services.AddAutoMapper();
            services.AddMediatR();

            services.AddScoped<IFileRepository, FileRepository>();


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
