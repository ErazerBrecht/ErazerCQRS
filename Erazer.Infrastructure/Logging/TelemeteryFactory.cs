using Erazer.Framework.Factories;
using Microsoft.ApplicationInsights;

namespace Erazer.Infrastructure.Logging
{
    public class TelemeteryFactory : IFactory<TelemetryClient>
    {
        public TelemetryClient Build()
        {
            return new TelemetryClient();
        }
    }
}
