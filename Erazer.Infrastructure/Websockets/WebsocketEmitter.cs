using System;
using System.Threading.Tasks;
using Erazer.Infrastructure.Logging;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.SeedWork;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Erazer.Infrastructure.Websockets
{
    public class WebsocketEmitter : IWebsocketEmitter
    {
        private readonly IHubContext<ReduxEventHub> _hubContext;
        private readonly ITelemetry _telemetryClient;
        private readonly ILogger<WebsocketEmitter> _logger;

        public WebsocketEmitter(IHubContext<ReduxEventHub> hubContext, ITelemetry telemetryClient, ILogger<WebsocketEmitter> logger)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Emit<T>(ReduxAction<T> action) where T : class
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
