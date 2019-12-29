namespace Erazer.Infrastructure.EventStore
{
    public interface IEventTypeMapping
    {
        string GetName<T>(T @event) where T : IDomainEvent;
        Type GetType(string eventName);
    }
}