using Erazer.Read.ViewModels.Ticket.Events.Enums;

namespace Erazer.Read.ViewModels.Ticket.Events
{
    public class TicketStatusEventViewModel : TicketEventViewModel
    {
        public TicketStatusEventViewModel()
        {
            Type = EventType.Status;
        }

        public StatusViewModel ToStatus { get; set; }
        public StatusViewModel FromStatus { get; set; }
    }
}