using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class SubscriptionJob : BackgroundService
    {
        private readonly ISubscription _subscription;
        private readonly IPositionRepository _positionRepository;
        private readonly ILogger<SubscriptionJob> _logger;

        public SubscriptionJob(ISubscription subscription, IPositionRepository positionRepository, ILogger<SubscriptionJob> logger, IApplicationLifetime lifetime)
        {
            _subscription = subscription;
            _positionRepository = positionRepository;
            _logger = logger;

            lifetime.ApplicationStopping.Register(() => _subscription.Dispose());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Started subscribing on EventStore");
                var position = await _positionRepository.GetCurrentPosistion();
                _subscription.Connect(position?.CheckPoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something failed starting eventstore subscriber");
                throw;
            }
        }
    }
}
