using System;
using Erazer.Framework.Factories;

namespace Erazer.Framework.Domain
{
    public class AggregateFactory<T>: IFactory<T> where T : AggregateRoot
    {
        public T Build()
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }
    }
}
