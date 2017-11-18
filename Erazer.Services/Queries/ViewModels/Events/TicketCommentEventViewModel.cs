namespace Erazer.Services.Queries.ViewModels.Events
{
    public class TicketCommentEventViewModel: TicketEventViewModel
    {
        public string Comment { get; set; }

        public TicketCommentEventViewModel()
        {
            Type = Domain.Constants.Enums.EventType.Comment;
        }
    }
}
