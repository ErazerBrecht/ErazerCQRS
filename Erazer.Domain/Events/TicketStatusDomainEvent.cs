using System;
using Erazer.Framework.Events;

namespace Erazer.Domain.Events
{
    public class TicketStatusDomainEvent : IDomainEvent
    {
        public string FromStatusId { get; set; }
        public string ToStatusId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }

        public Guid AggregateRootId { get; set; }
        public int Version { get; set; }

    }
}
