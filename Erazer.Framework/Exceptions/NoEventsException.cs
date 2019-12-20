using System;

namespace Erazer.Framework.Exceptions
{
    public class NoEventsException: Exception
    {
        public NoEventsException(Guid id) : base($"Aggregate {id} doesn't have any events to save!")
        {
            
        }
    }
}
