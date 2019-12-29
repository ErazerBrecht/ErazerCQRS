using System;
using Erazer.Read.ViewModels.Ticket.Events.Enums;

namespace Erazer.Read.ViewModels.Ticket.Events
{
    public abstract class TicketEventViewModel
    {
        public string Id { get; set; }
        public string TicketId { get; set; }
        public DateTime Created { get; set; }

        // TODO Add concept of users (identity)
        public string UserName => "ErazerBrecht";
        public EventType Type { get; protected set; }
    }
}