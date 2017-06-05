using System;
using Erazer.Services.Queries.DTOs.Events;

namespace Erazer.Services.Queries.DTOs
{
    public class TicketEventDto
    {
        public string Id { get; set; }
        public string TicketId { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }

        public BaseEventDto Event { get; set; }

    }
}
