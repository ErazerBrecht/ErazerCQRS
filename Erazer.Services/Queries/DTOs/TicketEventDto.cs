using System;
using Erazer.Domain.Constants.Enums;

namespace Erazer.Services.Queries.DTOs
{
    public class TicketEventDto
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public DateTime Created { get; set; }
        public Guid UserId { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public EventType Type { get; set; }
    }
}
