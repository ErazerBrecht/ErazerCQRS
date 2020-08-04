using System;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Infrastructure.Logging;
using Erazer.Read.Application.Infrastructure;
using Erazer.Syncing.Models;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class ComboSubscription: ISubscription
    {
        private readonly IDbQuery<SubscriptionDto> _subscriptionDbQuery;
        private readonly ISubscription _reSyncSubscription;
        private readonly ISubscription _liveSubscription;
        private readonly ILogger<ComboSubscription> _logger;

        public ComboSubscription(IEventStore eventStore, IDbQuery<SubscriptionDto> subscriptionDbQuery,
            IServiceProvider serviceProvider, ITelemetry telemetryClient, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            _subscriptionDbQuery = subscriptionDbQuery ?? throw new ArgumentNullException(nameof(subscriptionDbQuery));

            _reSyncSubscription = new ReSyncSubscription(eventStore, subscriptionDbQuery, serviceProvider, telemetryClient, loggerFactory.CreateLogger<ReSyncSubscription>());
            _liveSubscription = new LiveSubscription(eventStore, subscriptionDbQuery, serviceProvider, telemetryClient, loggerFactory.CreateLogger<LiveSubscription>());
            _logger = loggerFactory.CreateLogger<ComboSubscription>();
        }
        
        
        public void Dispose()
        {
            _reSyncSubscription.Dispose();
            _liveSubscription.Dispose();
        }

        public async Task Connect()
        {
            _logger.LogDebug("ComboSubscription started");
            var subscription = await _subscriptionDbQuery.Single(_ => true);

            if (subscription != null && subscription.Type == SubscriptionType.ReSync)
            {
                await _reSyncSubscription.Connect();
                _logger.LogDebug("'ReSync' subscription is done, switching to 'Live' subscription");
            }

            await _liveSubscription.Connect();
        }
    }
}