using Erazer.Framework.Factories;
using Microsoft.ApplicationInsights;

namespace Erazer.Web.Shared.Telemetery
{
    public class TelemeteryFactory : IFactory<TelemetryClient>
    {
        public TelemetryClient Build()
        {
            return new TelemetryClient();
        }
    }
}
