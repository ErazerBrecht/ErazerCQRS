using System;

namespace Erazer.DAL.EF.Entities
{
    public class TicketPriorityEventEntity 
    {
        public Guid Id { get; set; }
        public TicketEventEntity TicketEvent { get; set; }

        public int FromPriorityId { get; set; }
        public int ToPriorityId { get; set; }
    }
}
