using System;
using Erazer.Read.ViewModels.Ticket.Events;

namespace Erazer.Syncing.SeedWork.Redux
{
    public class ReduxUpdateStatusAction : ReduxAction<TicketStatusEventViewModel>
    {
        public string TicketId { get; }
        
        public ReduxUpdateStatusAction(string ticketId, TicketStatusEventViewModel payload) : base(ReduxActionTypeConstants.UpdateTicketStatus, payload)
        {
            TicketId = ticketId ?? throw new ArgumentNullException(nameof(ticketId));
        }
    }
}
