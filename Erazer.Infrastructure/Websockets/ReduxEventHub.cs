using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.Websockets
{
    public class ReduxEventHub : Hub
    {
        private readonly ILogger<ReduxEventHub> _logger;

        public ReduxEventHub(ILogger<ReduxEventHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _logger.LogDebug($"Debug: Client connected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            _logger.LogDebug(exception, $"Debug: Client disconnected");
        }
    }
}
