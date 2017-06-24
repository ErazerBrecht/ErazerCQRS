using Erazer.Framework.Events;
using System;

namespace Erazer.Domain.Events
{
    public class TicketCreateEvent: IEvent
    {
        public TicketCreateEvent(Guid ticketId)
        {
            AggregateRootId = ticketId;
        }
        public string Title { get; set; }
        public string Description { get; set; }

        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public Guid AggregateRootId { get; set; }
        public int Version { get; set; }
    }
}
