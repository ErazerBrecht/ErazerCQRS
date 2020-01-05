using Erazer.Read.ViewModels.Ticket.Events;

namespace Erazer.Syncing.SeedWork.Redux
{
    public class ReduxCommentAddedAction : ReduxAction<TicketCommentEventViewModel>
    {
        public ReduxCommentAddedAction(TicketCommentEventViewModel payload) : base(ReduxActionTypeConstants.AddTicketComment, payload)
        {
        }
    }
}
