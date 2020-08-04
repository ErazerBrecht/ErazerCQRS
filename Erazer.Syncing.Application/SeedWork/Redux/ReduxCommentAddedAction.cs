using System;
using Erazer.Read.ViewModels.Ticket.Events;

namespace Erazer.Syncing.SeedWork.Redux
{
    public class ReduxCommentAddedAction : ReduxAction<TicketCommentEventViewModel>
    {
        public string TicketId { get; }
        
        public ReduxCommentAddedAction(string ticketId, TicketCommentEventViewModel payload) : base(ReduxActionTypeConstants.AddTicketComment, payload)
        {
            TicketId = ticketId ?? throw new ArgumentNullException(nameof(ticketId));
        }
    }
}
