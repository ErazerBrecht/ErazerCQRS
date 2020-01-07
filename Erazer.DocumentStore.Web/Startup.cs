using System;
using System.Reflection;
using Erazer.DocumentStore.Application.DTOs;
using Erazer.DocumentStore.Application.Infrastructure;
using Erazer.DocumentStore.Application.Query;
using Erazer.Infrastructure.DocumentStore.Repositories;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Messages.Commands;
using Erazer.Messages.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            services.AddSingletonFactory<ITelemetry, TelemetryFactory>();
            services.AddMongo(_configuration.GetSection("MongoDbSettings"), x=>
            {
                x.AddAssembly(typeof(FileContentDto).GetTypeInfo().Assembly);
                x.Dto<FileContentDto>(d => d.SetCollectionName("Files"));
            });
            
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddMediatR
            (
                typeof(FileRequest).GetTypeInfo().Assembly,
                typeof(UploadFileCommand).GetTypeInfo().Assembly
            );
            
            // Add MVC
            services.AddControllers();

            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document API", Version = "v1" });
            });
            
            // Add ServiceBus
            services.AddBus(x =>
            {
                x.ConnectionString = _busSettings.ConnectionString;
                x.ConnectionName = "Erazer.Web.DocumentStore";
                x.UserName = _busSettings.UserName;
                x.Password = _busSettings.Password;
                x.AddCommands(y =>
                {
                    y.QueueName = CommandBusEndPoints.ErazerDocumentStore;
                    y.AddCommandListener<UploadFileCommand>();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document API");
                });
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200") // Load this from ENV or Config file
                    .WithMethods("GET")
                    .AllowAnyHeader()
                    .SetPreflightMaxAge(TimeSpan.FromHours(1));
            });
            
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}