using System;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Infrastructure.Logging;
using Erazer.Read.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class ComboSubscription: ISubscription
    {
        private readonly ISubscription _reSyncSubscription;
        private readonly ISubscription _liveSubscription;
        private readonly ILogger<ComboSubscription> _logger;
        
        public ComboSubscription(IEventStore eventStore, IDbQuery<PositionDto> positionDbQuery, IServiceCollection serviceCollection,
            ITelemetry telemetryClient, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            
            _reSyncSubscription = new ReSyncSubscription(eventStore, positionDbQuery, serviceCollection, telemetryClient, loggerFactory.CreateLogger<ReSyncSubscription>());
            _liveSubscription = new LiveSubscription(eventStore, positionDbQuery, serviceCollection, telemetryClient, loggerFactory.CreateLogger<LiveSubscription>());
            _logger = loggerFactory.CreateLogger<ComboSubscription>();
        }
        
        
        public void Dispose()
        {
            _reSyncSubscription.Dispose();
            _liveSubscription.Dispose();
        }

        public async Task Connect()
        {
            _logger.LogDebug("ComboSubscription started, first executing 'ReSync' subscription");
            await _reSyncSubscription.Connect();
            _logger.LogDebug("'ReSync' subscription is done, switching to 'Live' subscription");
            await _liveSubscription.Connect();
        }
    }
}