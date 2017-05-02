using System;
using System.Collections.Generic;
using System.Text;
using Erazer.DAL.ReadModel.Enums;

namespace Erazer.DAL.ReadModel
{
    public class TicketEventDto
    {
        public Guid Id { get; set; }
        public string TicketId { get; set; }
        public DateTime Created { get; set; }
        public Guid UserId { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public EventType Type { get; set; }
    }
}
