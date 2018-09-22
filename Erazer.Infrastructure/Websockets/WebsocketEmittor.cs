using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;
using System;
using Erazer.Framework.FrontEnd;
using Erazer.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.Websockets
{
    public class WebsocketEmittor : IWebsocketEmittor
    {
        private readonly IHubContext<ReduxEventHub> _hubContext;
        private readonly TelemetryClient _telemeteryClient;
        private readonly ILogger<WebsocketEmittor> _logger;

        public WebsocketEmittor(IHubContext<ReduxEventHub> hubContext, TelemetryClient telemeteryClient, ILogger<WebsocketEmittor> logger)
        {
            _hubContext = hubContext;
            _telemeteryClient = telemeteryClient;
            _logger = logger;
        }

        public async Task Emit<T>(ReduxAction<T> action) where T : IViewModel
        {
            var jsonString = JsonConvert.SerializeObject(action, JsonSettings.CamelCaseSerializer);
            var now = DateTime.Now;

            try
            {
                await _hubContext.Clients.All.SendAsync("SendAction", jsonString);
                _telemeteryClient.TrackDependency("SignalR", "Websocket 'push-all'", $"ReduxEmit - {action.Type}", now, DateTime.Now - now, true);
            }
            catch (Exception ex)
            {
                _telemeteryClient.TrackDependency("SignalR", "Websocket 'push-all'", $"ReduxEmit - {action.Type}", now, DateTime.Now - now, false);
                _logger.LogError(ex, $"Failed to push 'ReduxAction - {action.Type}' with websocket connection");
                throw;
            }
        }
    }
}
