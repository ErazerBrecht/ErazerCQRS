using System;
using System.Collections.Generic;
using Erazer.Messages.Commands;

namespace Erazer.Infrastructure.ServiceBus.Commands
{
    public class CommandConfig : IConsumerConfig
    {
        public IList<Type> Listeners { get; } = new List<Type>();
        public string QueueName { get; set; }
        public ushort? PrefetchCount { get; set; }
        public bool Durable => true;

        public void AddCommandListener<T>() where T : class, ICommand
        {
            var type = typeof(CommandConsumer<T>);
            Listeners.Add(type);
        }
    }
}