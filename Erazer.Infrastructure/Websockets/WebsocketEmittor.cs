using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;
using System;
using Erazer.Framework.FrontEnd;
using Erazer.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Erazer.Infrastructure.Websockets
{
    public class WebsocketEmittor : IWebsocketEmittor
    {
        private readonly IHubContext<ReduxEventHub, IReduxHub> _hubContext;
        private readonly TelemetryClient _telemeteryClient;

        public WebsocketEmittor(IHubContext<ReduxEventHub, IReduxHub> hubContext, TelemetryClient telemeteryClient)
        {
            _hubContext = hubContext;
            _telemeteryClient = telemeteryClient;
        }

        public async Task TestEmit()
        {
            var now = DateTime.Now;

            try
            {
                await _hubContext.Clients.All.SendAction("Lol");
                _telemeteryClient.TrackDependency("SignalR", "Websocket 'push-all'", $"TestEmit", now, DateTime.Now - now, true);
            }
            catch
            {
                _telemeteryClient.TrackDependency("SignalR", "Websocket 'push-all'", $"TestEmit", now, DateTime.Now - now, false);
                throw;
            }
        }

        public async Task Emit<T>(ReduxAction<T> action) where T : IViewModel
        {
            var jsonString = JsonConvert.SerializeObject(action, JsonSettings.CamelCaseSerializer);
            var now = DateTime.Now;

            try
            {
                await _hubContext.Clients.All.SendAction("Lol");
                _telemeteryClient.TrackDependency("SignalR", "Websocket 'push-all'", $"ReduxEmit - {action.Type}", now, DateTime.Now - now, true);
            }
            catch
            {
                _telemeteryClient.TrackDependency("SignalR", "Websocket 'push-all'", $"ReduxEmit - {action.Type}", now, DateTime.Now - now, false);
                throw;
            }
        }
    }
}
