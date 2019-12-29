namespace Erazer.Infrastructure.Logging
{
    public class TelemetryAppInsights : ITelemetry
    {
        private readonly TelemetryClient _client;

        public TelemetryAppInsights(TelemetryClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public void TrackTrace(string message)
        {
            _client.TrackTrace(message);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            _client.TrackEvent(eventName, properties, metrics);
        }

        public void TrackSpan(string name, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            _client.TrackRequest(name, startTime, duration, "N/A", success);
        }

        public void TrackDependency(string dependencyTypeName, string dependencyName, string data,
            DateTimeOffset startTime,
            TimeSpan duration, bool success)
        {
            _client.TrackDependency(dependencyTypeName, dependencyName, data, startTime, duration, success);
        }
    }
}