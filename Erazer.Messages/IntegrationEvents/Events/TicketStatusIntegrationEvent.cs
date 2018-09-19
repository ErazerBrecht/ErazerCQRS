using System;

namespace Erazer.Messages.IntegrationEvents.Events
{
    public class TicketStatusIntegrationEvent : IIntegrationEvent
    {
        public string StatusId { get; }
        public string StatusName { get; }

        public string TicketId { get; }
        public string TicketTitle { get; }


        public string CreateEventId { get; }
        public DateTime Created { get; }
        public string UserId { get; }


        public TicketStatusIntegrationEvent(string statusId, string statusName, string ticketId, string ticketTitle,
            string createEventId, DateTime created, string userId)
        {
            StatusId = statusId;
            StatusName = statusName;
            TicketId = ticketId;
            TicketTitle = ticketTitle;
            CreateEventId = createEventId;
            Created = created;
            UserId = userId;
        }
    }
}
