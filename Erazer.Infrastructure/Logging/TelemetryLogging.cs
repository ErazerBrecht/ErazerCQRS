namespace Erazer.Infrastructure.Logging
{
    public class TelemetryLogging : ITelemetry
    {
        private readonly ILogger<TelemetryLogging> _logger;

        public TelemetryLogging(ILogger<TelemetryLogging> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void TrackTrace(string message)
        {
            _logger.LogTrace(message);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            if (metrics != null)
            {
                var stringMetrics = string.Join(";", metrics.Select(x => x.Key + "=" + x.Value));
                var stringProperties = string.Join(";", properties.Select(x => x.Key + "=" + x.Value));

                _logger.LogInformation($"Event -> {eventName}: {stringProperties}, metric: {stringMetrics}");
            }
            else if (properties != null)
            {
                var stringProperties = string.Join(";", properties.Select(x => x.Key + "=" + x.Value));
                _logger.LogInformation($"Event -> {eventName}: {stringProperties}");
            }
            else
            {
                _logger.LogInformation($"Event -> {eventName}");
            }
        }

        public void TrackSpan(string name, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            if (success)
                _logger.LogInformation(
                    $"Span -> {name}: {data}, completed in: {duration.TotalMilliseconds}ms");
            else
                _logger.LogError(
                    $"Span -> {name}: {data}, failed in: {duration.TotalMilliseconds}ms");
        }

        public void TrackDependency(string dependencyTypeName, string dependencyName, string data,
            DateTimeOffset startTime,
            TimeSpan duration, bool success)
        {
            if (success)
                _logger.LogInformation(
                    $"Dependency -> {dependencyTypeName} - {dependencyName}: {data}, completed in: {duration.TotalMilliseconds}ms");
            else
                _logger.LogWarning(
                    $"Dependency -> {dependencyTypeName} - {dependencyName}: {data}, failed in: {duration.TotalMilliseconds}ms");
        }
    }
}