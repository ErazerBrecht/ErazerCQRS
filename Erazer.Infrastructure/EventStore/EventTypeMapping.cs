namespace Erazer.Infrastructure.EventStore
{
    public class EventTypeMapping : IEventTypeMapping
    {
        private readonly IReadOnlyDictionary<string, Type> _map;

        public EventTypeMapping()
        {
            var type = typeof(IDomainEvent);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();

            _map = types.Select(t => new {Type = t, Name = GetEventName(t)}).ToDictionary(t => t.Name, t => t.Type);
        }

        public string GetName<T>(T @event) where T : IDomainEvent
        {
            return _map.Single(m => m.Value == @event.GetType()).Key;
        }

        public Type GetType(string eventName)
        {
            return _map[eventName];
        }

        private static string GetEventName(MemberInfo type)
        {
            var attribute = type.GetCustomAttribute<EventAttribute>();
            return attribute?.Name ?? type.Name;
        }
    }
}
