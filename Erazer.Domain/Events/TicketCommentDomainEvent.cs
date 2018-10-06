using System;
using Erazer.Framework.Events;

namespace Erazer.Domain.Events
{
    [Event("TicketCommentEvent")]
    public class TicketCommentDomainEvent : IDomainEvent
    {
        public string Comment { get; set; }

        public Guid UserId { get; set; }
        public DateTime Created { get; set; }

        public Guid AggregateRootId { get; set; }
        public int Version { get; set; }
    }
}
