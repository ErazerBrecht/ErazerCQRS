using Erazer.Services.Queries.ViewModels.Events.Enums;

namespace Erazer.Services.Queries.ViewModels.Events
{
    public class TicketCommentEventViewModel: TicketEventViewModel
    {
        public string Comment { get; set; }

        public TicketCommentEventViewModel()
        {
            Type = EventType.Comment;
        }
    }
}
