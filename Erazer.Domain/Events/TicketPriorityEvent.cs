using System;
using Erazer.Framework.Events;

namespace Erazer.Domain.Events
{
    public class TicketPriorityEvent : IEvent
    {
        public int FromPriorityId { get; set; }
        public int ToPriorityId { get; set; }

        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public Guid AggregateRootId { get; set; }
    }
}
