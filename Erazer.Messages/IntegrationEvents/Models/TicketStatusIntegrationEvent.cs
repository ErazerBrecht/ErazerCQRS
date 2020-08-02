using System.Diagnostics.CodeAnalysis;

namespace Erazer.Messages.IntegrationEvents.Models
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class TicketStatusIntegrationEvent : IIntegrationEvent
    {
        public string StatusId { get; private set; }
        public string StatusName { get; private set; }

        public string TicketId { get; private set; }
        public string TicketTitle { get; private set; }

        public string CreateEventId { get; private set; }
        public long Created { get; private set; }

        private TicketStatusIntegrationEvent()
        {
        }

        public TicketStatusIntegrationEvent(string statusId, string statusName, string ticketId, string ticketTitle,
            string createEventId, long created)
        {
            StatusId = statusId;
            StatusName = statusName;
            TicketId = ticketId;
            TicketTitle = ticketTitle;
            CreateEventId = createEventId;
            Created = created;
        }
    }
}