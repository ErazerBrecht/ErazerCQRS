using System;

namespace Erazer.Framework.Exceptions
{
    public class EventsOfWrongAggregateException: Exception
    {
        public EventsOfWrongAggregateException(Guid aggregateId, Guid aggregateIdEvents): 
            base($"Tried to load events of another aggregate {aggregateIdEvents} to this aggregate {aggregateId}")
        {
        }
    }
}