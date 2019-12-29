using Erazer.Read.ViewModels.Ticket.Events.Enums;

namespace Erazer.Read.ViewModels.Ticket.Events
{
    public class TicketCreatedEventViewModel : TicketEventViewModel
    {
        public TicketCreatedEventViewModel()
        {
            Type = EventType.Created;
        }
    }
}