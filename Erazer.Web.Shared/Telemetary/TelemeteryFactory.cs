using Erazer.Framework.Factories;
using Microsoft.ApplicationInsights;

namespace Erazer.Web.Shared.Telemetary
{
    public class TelemeteryFactory : IFactory<TelemetryClient>
    {
        public TelemetryClient Build()
        {
            return new TelemetryClient();
        }
    }
}
