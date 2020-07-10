using Erazer.Syncing.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Infrastructure.Websockets
{
    public static class WebsocketExtensions
    {
        public static IServiceCollection AddWebsocketEmitter(this IServiceCollection services)
        {
            services.AddScoped<IWebsocketEmitter, WebsocketEmitter>();
            services.AddSignalR();

            return services;
        }
        
        public static void UseWebsocketEmitter(this IApplicationBuilder app)
        {
            app.UseSignalR(routes => { routes.MapHub<ReduxEventHub>("/events"); });
        }
    }
}
