using Erazer.Read.ViewModels.Ticket.Events.Enums;

namespace Erazer.Read.ViewModels.Ticket.Events
{
    public class TicketCommentEventViewModel : TicketEventViewModel
    {
        public TicketCommentEventViewModel()
        {
            Type = EventType.Comment;
        }

        public string Comment { get; set; }
    }
}