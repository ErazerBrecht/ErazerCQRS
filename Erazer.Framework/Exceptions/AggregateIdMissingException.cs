using System;

namespace Erazer.Framework.Exceptions
{
    public class AggregateIdMissingException: Exception
    {
        public AggregateIdMissingException(Type type):  base($"Aggregate of type '{type.FullName}' doesn't have an id when trying to save!")
        {
        }
    }
}