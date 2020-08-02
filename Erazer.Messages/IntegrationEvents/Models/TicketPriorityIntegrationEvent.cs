using System;
using System.Diagnostics.CodeAnalysis;

namespace Erazer.Messages.IntegrationEvents.Models
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class TicketPriorityIntegrationEvent : IIntegrationEvent
    {
        public string PriorityId { get; private set; }
        public string PriorityName { get; private set; }

        public string TicketId { get; private set; }
        public string TicketTitle { get; private set; }


        public string CreateEventId { get; private set; }
        public long Created { get; private set; }


        private TicketPriorityIntegrationEvent()
        {
        }

        public TicketPriorityIntegrationEvent(string priorityId, string priorityName, string ticketId,
            string ticketTitle, string createEventId, long created)
        {
            PriorityId = priorityId;
            PriorityName = priorityName;
            TicketId = ticketId;
            TicketTitle = ticketTitle;
            CreateEventId = createEventId;
            Created = created;
        }
    }
}