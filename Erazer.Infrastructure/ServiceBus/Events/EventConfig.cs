using System;
using System.Collections.Generic;
using Erazer.Messages.IntegrationEvents;

namespace Erazer.Infrastructure.ServiceBus.Events
{
    public class EventConfig : IConsumerConfig
    {
        public IList<Type> Listeners { get; } = new List<Type>();
        public string QueueName { get; set; }
        public ushort? PrefetchCount { get; set; }
        public bool Durable => true;

        public void AddEventListener<T>() where T : class, IIntegrationEvent
        {
            var type = typeof(EventConsumer<T>);
            Listeners.Add(type);
        }
    }
}