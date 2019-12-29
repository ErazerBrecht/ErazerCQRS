namespace Erazer.Infrastructure.Logging
{
    public class TelemeteryFactory : IFactory<ITelemetry>
    {
        private readonly ILogger<TelemetryLogging> _logger;

        public TelemeteryFactory(ILogger<TelemetryLogging> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public ITelemetry Build()
        {
            return new TelemetryLogging(_logger);
        }
    }
}
