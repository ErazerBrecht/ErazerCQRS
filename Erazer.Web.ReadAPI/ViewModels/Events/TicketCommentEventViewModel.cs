using Erazer.Web.ReadAPI.ViewModels.Events.Enums;

namespace Erazer.Web.ReadAPI.ViewModels.Events
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
