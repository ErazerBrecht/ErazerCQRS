using System;
using System.Collections.Generic;

namespace Erazer.Infrastructure.Logging
{
    public interface ITelemetry
    {
        void TrackTrace(string message);

        void TrackEvent(
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);

        void TrackDependency(
            string dependencyTypeName,
            string dependencyName,
            string data,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success);
    }
}