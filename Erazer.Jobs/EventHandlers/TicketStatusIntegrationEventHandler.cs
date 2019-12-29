using Erazer.Messages.IntegrationEvents.Models;

namespace Erazer.Jobs.EventHandlers
{
    public class TicketStatusIntegrationEventHandler : NotificationHandler<TicketStatusIntegrationEvent>
    {
        protected override void HandleCore(TicketStatusIntegrationEvent notification)
        {
            Console.WriteLine("Status of a ticket was changed!");
            Console.WriteLine($"Ticket title: {notification.TicketTitle}");
            Console.WriteLine($"New priority: {notification.StatusName}");
        }
    }
}
