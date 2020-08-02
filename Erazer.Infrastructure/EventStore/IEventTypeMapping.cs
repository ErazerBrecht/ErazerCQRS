using System;
using Erazer.Framework.Events;

namespace Erazer.Infrastructure.EventStore
{
    public interface IEventTypeMapping
    {
        string GetName<T>(T @event) where T : IEvent;
        Type GetType(string eventName);
    }
}