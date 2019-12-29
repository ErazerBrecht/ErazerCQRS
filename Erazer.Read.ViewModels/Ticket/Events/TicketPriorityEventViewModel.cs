using Erazer.Read.ViewModels.Ticket.Events.Enums;

namespace Erazer.Read.ViewModels.Ticket.Events
{
    public class TicketPriorityEventViewModel : TicketEventViewModel
    {
        public TicketPriorityEventViewModel()
        {
            Type = EventType.Priority;
        }

        public PriorityViewModel ToPriority { get; set; }
        public PriorityViewModel FromPriority { get; set; }
    }
}