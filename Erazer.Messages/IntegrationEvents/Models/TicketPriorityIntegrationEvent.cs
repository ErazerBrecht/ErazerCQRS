using System;

namespace Erazer.Messages.IntegrationEvents.Models
{
    public class TicketPriorityIntegrationEvent : IIntegrationEvent
    {
        public string PriorityId { get; }
        public string PriorityName { get; }

        public string TicketId { get; }
        public string TicketTitle { get; }


        public string CreateEventId { get; }
        public DateTime Created { get; }
        public string UserId { get; }


        public TicketPriorityIntegrationEvent(string priorityId, string priorityName, string ticketId, string ticketTitle,
            string createEventId, DateTime created, string userId)
        {
            PriorityId = priorityId;
            PriorityName = priorityName;
            TicketId = ticketId;
            TicketTitle = ticketTitle;
            CreateEventId = createEventId;
            Created = created;
            UserId = userId;
        }
    }
}
