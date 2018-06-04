using System;
using System.Threading.Tasks;
using Erazer.Framework.FrontEnd;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.Websockets
{
    public class ReduxEventHub : Hub<IReduxHub>
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

    public interface IReduxHub
    {
       Task SendAction(string action);
    }
}
