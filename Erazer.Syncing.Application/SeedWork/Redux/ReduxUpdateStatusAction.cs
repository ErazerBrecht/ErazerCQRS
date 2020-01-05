using Erazer.Read.ViewModels.Ticket.Events;

namespace Erazer.Syncing.SeedWork.Redux
{
    public class ReduxUpdateStatusAction : ReduxAction<TicketStatusEventViewModel>
    {
        public ReduxUpdateStatusAction(TicketStatusEventViewModel payload) : base(ReduxActionTypeConstants.UpdateTicketStatus, payload)
        {
        }
    }
}
