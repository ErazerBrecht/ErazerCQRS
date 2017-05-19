using System;

namespace Erazer.Domain.Events
{
    public class TicketCommentEvent : IEvent
    {
        public string Comment { get; set; }

        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public Guid AggregateRootId { get; set; }
    }
}
