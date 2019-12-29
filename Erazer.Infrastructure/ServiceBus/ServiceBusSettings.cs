using Erazer.Infrastructure.ServiceBus.Commands;
using Erazer.Infrastructure.ServiceBus.Events;

namespace Erazer.Infrastructure.ServiceBus
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public string ConnectionName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public IConsumerConfig EventConfig { get; private set; }
        public IConsumerConfig CommandConfig { get; private set; }

        public void AddEvents(Action<EventConfig> configureEvents)
        {
            var x = new EventConfig();
            configureEvents.Invoke(x);
            EventConfig = x;
        }

        public void AddCommands(Action<CommandConfig> configureCommands)
        {
            var y = new CommandConfig();
            configureCommands.Invoke(y);
            CommandConfig = y;
        }
    }

    public interface IConsumerConfig
    {
        IList<Type> Listeners { get; }
        string QueueName { get; }
        ushort? PrefetchCount { get; }
        bool Durable { get; }
    }
    
}
