using System;

namespace Erazer.Framework.Domain
{
    public static class AggregateFactory
    {
        public static T Build<T>() where T : AggregateRoot
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }
    }
}
