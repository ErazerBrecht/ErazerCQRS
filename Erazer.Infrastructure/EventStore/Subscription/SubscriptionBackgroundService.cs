namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class SubscriptionBackgroundService : BackgroundService
    {
        private readonly ISubscription _subscription;
        private readonly ILogger<SubscriptionBackgroundService> _logger;

        public SubscriptionBackgroundService(ISubscription subscription, ILogger<SubscriptionBackgroundService> logger)
        {
            _subscription = subscription;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogDebug("Started subscribing on EventStore");
                await _subscription.Connect();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something failed starting eventstore subscriber");
                throw;
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Stop subscribing on EventStore");

            _subscription?.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
