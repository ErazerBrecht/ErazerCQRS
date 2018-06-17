namespace Erazer.Infrastructure.ServiceBus
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class EventServiceBusSettings: ServiceBusSettings
    {
        public string EventQueueName { get; set; }
    }

    public class CommandServiceBusSettings : ServiceBusSettings
    {
        public string CommandQueueName { get; set; }
    }
}
