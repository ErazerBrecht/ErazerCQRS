using AutoMapper;
using Erazer.DAL.EF;
using Erazer.DAL.EF.Repositories;
using Erazer.DAL.ReadModel.AggregateRepositories;
using Erazer.DAL.ReadModel.Base;
using Erazer.DAL.ReadModel.Repositories;
using Erazer.Domain;
using Erazer.Framework.Domain.Repositories;
using Erazer.Framework.Events;
using Erazer.Services.Queries.Repositories;
using Erazer.Web.Extensions;
using Erazer.Web.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Erazer.Web
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

            // Add DB Providers
            services.AddDbContext<ErazerEventContext>(options => options.UseSqlServer(_configuration.GetConnectionString("Erazer.Database")));
            services.AddScopedFactory<IMongoDatabase, MongoDbFactory>();

            services.AddAutoMapper();
            services.AddMediatR();

            // TODO Place in seperate file
            // Query repositories
            services.AddScoped<ITicketQueryRepository, TicketRepository>();
            services.AddScoped<ITicketEventQueryRepository, TicketEventRepository>();
            services.AddScoped<IStatusQueryRepository, StatusRepository>();
            services.AddScoped<IPriorityQueryRepository, PriorityRepository>();

            // Aggregate repositories
            services.AddScoped<IAggregateRepository<Ticket>, TicketAggregrateRepository>();

            // Event repositories
            services.AddScoped<IEventRepository, EventRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(_configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMongoDbClassMaps();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
