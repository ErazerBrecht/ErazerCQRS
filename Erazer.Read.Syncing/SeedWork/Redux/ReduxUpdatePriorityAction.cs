using Erazer.Read.ViewModels.Ticket.Events;

namespace Erazer.Syncing.SeedWork.Redux
{
    public class ReduxUpdatePriorityAction : ReduxAction<TicketPriorityEventViewModel>
    {
        public ReduxUpdatePriorityAction(TicketPriorityEventViewModel payload) : base(ReduxActionTypeConstants.UpdateTicketPriority, payload)
        {
        }
    }
}
