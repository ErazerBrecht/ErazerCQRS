using System;
using Erazer.Messages.IntegrationEvents.Models;
using MediatR;

namespace Erazer.Jobs.EventHandlers
{
    public class TicketCreatedIntegrationEventHandler: NotificationHandler<TicketCreatedIntegrationEvent>
    {
        protected override void Handle(TicketCreatedIntegrationEvent notification)
        {
            Console.WriteLine("New ticket has been created!");
            Console.WriteLine($"Title {notification.Title}");
        }
    }
}
