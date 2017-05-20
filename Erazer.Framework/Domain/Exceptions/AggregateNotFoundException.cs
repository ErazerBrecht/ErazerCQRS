using System;

namespace Erazer.Framework.Domain.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(Type t, Guid id) : base($"Aggregate {id} of type {t.FullName} was not found")
        {
            
        }
    }
}
