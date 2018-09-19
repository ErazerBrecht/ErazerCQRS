using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.EventStore.PersistedSubscription
{
    public class SubscriptionJob : BackgroundService
    {
        private readonly ISubscription _subscription;
        private readonly ILogger<SubscriptionJob> _logger;

        public SubscriptionJob(ISubscription subscription, ILogger<SubscriptionJob> logger, IApplicationLifetime lifetime)
        {
            _subscription = subscription;
            _logger = logger;

            lifetime.ApplicationStopping.Register(() => _subscription.Dispose());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Started subscribing on EventStore");
            await Task.Run(() => _subscription.Connect(), stoppingToken);
        }
    }
}
