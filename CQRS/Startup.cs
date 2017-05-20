using AutoMapper;
using Erazer.DAL.Dapper.AggregateRepositories;
using Erazer.DAL.Dapper.QueryRepositories;
using Erazer.DAL.EF;
using Erazer.DAL.EF.Repositories;
using Erazer.Domain;
using Erazer.Framework.Domain;
using Erazer.Services.Events.Repositories;
using Erazer.Services.Queries.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Erazer.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            // Add EF
            services.AddDbContext<ErazerEventContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Erazer.Database")));

            // TODO Place in seperate file
            services.AddAutoMapper();
            services.AddMediatR();

            // Query repositories
            services.AddScoped<ITicketQueryRepository, TicketQueryRepository>();
            services.AddScoped<ITicketEventQueryRepository, TicketEventQueryRepository>();
            services.AddScoped<IStatusQueryRepository, StatusQueryRepository>();
            services.AddScoped<IPriorityQueryRepository, PriorityQueryRepository>();

            // Aggregate repositories
            services.AddScoped<IAggregateRepository<Ticket>, TicketAggregrateRepository>();

            // Event repositories
            services.AddScoped<ITicketCommentEventRepository, TicketCommentEventRepository>();


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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
