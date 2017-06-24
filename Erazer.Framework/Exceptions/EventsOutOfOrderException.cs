using System;

namespace Erazer.Framework.Exceptions
{
    public class EventsOutOfOrderException : Exception
    {
        public EventsOutOfOrderException(Guid id) : base($"Eventstore gave event for aggregate {id} out of order")
        {
            
        }
    }
}
