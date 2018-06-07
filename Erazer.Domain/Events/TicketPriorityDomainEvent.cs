using System;
using Erazer.Framework.Events;

namespace Erazer.Domain.Events
{
    public class TicketPriorityDomainEvent : IDomainEvent
    {
        public string FromPriorityId { get; set; }
        public string ToPriorityId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }

        public Guid AggregateRootId { get; set; }
        public int Version { get; set; }

    }
}
