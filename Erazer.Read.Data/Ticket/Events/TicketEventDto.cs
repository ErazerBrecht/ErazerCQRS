using System;
using Erazer.Framework.DTO;

namespace Erazer.Read.Data.Ticket.Events
{
    public abstract class TicketEventDto : IDto
    {
        public string Id { get; set; }
        public string TicketId { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
    }
}