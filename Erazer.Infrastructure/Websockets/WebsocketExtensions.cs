using Microsoft.AspNetCore.Builder;

namespace Erazer.Infrastructure.Websockets
{
    public static class WebsocketExtensions
    {
        public static void UseWebsocketEmittor(this IApplicationBuilder app)
        {
            app.UseSignalR(routes => { routes.MapHub<ReduxEventHub>("/events"); });
        }
    }
}
