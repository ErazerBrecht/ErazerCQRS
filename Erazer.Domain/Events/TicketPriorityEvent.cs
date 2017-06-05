using System;
using Erazer.Framework.Events;

namespace Erazer.Domain.Events
{
    public class TicketPriorityEvent : IEvent
    {
        public string FromPriorityId { get; set; }
        public string ToPriorityId { get; set; }

        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public Guid AggregateRootId { get; set; }
    }
}
