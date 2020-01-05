using Erazer.Domain.Files;
using Erazer.Framework.Events;
using System;
using System.Collections.Generic;

namespace Erazer.Domain.Events
{
    [Event("TicketCreateEvent")]
    public class TicketCreateDomainEvent: IDomainEvent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PriorityId { get; set; }
        public string StatusId { get; set; }
        public List<File> Files { get; set; }

        public Guid AggregateRootId { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public Guid? UserId { get; set; }
        
        public TicketCreateDomainEvent(Guid ticketId)
        {
            AggregateRootId = ticketId;
        }
    }
}
