using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.EventStore.PersistedSubscription
{
    public class PersistedSubscriptionHost<T> : BackgroundService where T : AggregateRoot
    {
        private readonly ISubscription<T> _subscription;
        private readonly ILogger<PersistedSubscriptionHost<T>> _logger;

        public PersistedSubscriptionHost(ISubscription<T> subscription, ILogger<PersistedSubscriptionHost<T>> logger, IApplicationLifetime lifetime)
        {
            _subscription = subscription;
            _logger = logger;

            lifetime.ApplicationStopping.Register(() => _subscription.Dispose());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _subscription.Connect();
        }
    }
}
