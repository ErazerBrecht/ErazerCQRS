using System;
using Erazer.Read.ViewModels.Ticket.Events;

namespace Erazer.Syncing.SeedWork.Redux
{
    public class ReduxUpdatePriorityAction : ReduxAction<TicketPriorityEventViewModel>
    {
        public string TicketId { get; }

        public ReduxUpdatePriorityAction(string ticketId, TicketPriorityEventViewModel payload) : base(ReduxActionTypeConstants.UpdateTicketPriority, payload)
        {
            TicketId = ticketId ?? throw new ArgumentNullException(nameof(ticketId));
        }
    }
}
