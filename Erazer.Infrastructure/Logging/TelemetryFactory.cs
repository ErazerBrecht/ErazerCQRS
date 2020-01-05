using System;
using Erazer.Framework.Factories;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.Logging
{
    public class TelemetryFactory : IFactory<ITelemetry>
    {
        private readonly ILogger<TelemetryLogging> _logger;

        public TelemetryFactory(ILogger<TelemetryLogging> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public ITelemetry Build()
        {
            return new TelemetryLogging(_logger);
        }
    }
}
