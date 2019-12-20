using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using Erazer.Framework.FrontEnd;
using Erazer.Infrastructure.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.Websockets
{
    public class WebsocketEmittor : IWebsocketEmittor
    {
        private readonly IHubContext<ReduxEventHub> _hubContext;
        private readonly ITelemetry _telemetryClient;
        private readonly ILogger<WebsocketEmittor> _logger;

        public WebsocketEmittor(IHubContext<ReduxEventHub> hubContext, ITelemetry telemetryClient, ILogger<WebsocketEmittor> logger)
        {
            _hubContext = hubContext;
            _telemetryClient = telemetryClient;
            _logger = logger;
        }

        public async Task Emit<T>(ReduxAction<T> action) where T : IViewModel
        {
            var jsonString = JsonConvert.SerializeObject(action, JsonSettings.JavascriptSerializer);
            var now = DateTime.Now;

            try
            {
                await _hubContext.Clients.All.SendAsync("SendAction", jsonString);
                _telemetryClient.TrackDependency("SignalR", "Websocket 'push-all'", $"ReduxEmit - {action.Type}", now, DateTime.Now - now, true);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackDependency("SignalR", "Websocket 'push-all'", $"ReduxEmit - {action.Type}", now, DateTime.Now - now, false);
                _logger.LogError(ex, $"Failed to push 'ReduxAction - {action.Type}' with websocket connection");
                throw;
            }
        }
    }
}
