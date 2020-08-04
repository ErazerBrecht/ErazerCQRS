using Erazer.Syncing.Infrastructure;
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
    }
}
