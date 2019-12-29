using Erazer.Read.ViewModels.Ticket;

namespace Erazer.Syncing.SeedWork.Redux
{
    public class ReduxTicketCreateAction : ReduxAction<TicketViewModel>
    {
        public ReduxTicketCreateAction(TicketViewModel payload) : base(ReduxActionTypeConstants.AddTicket, payload)
        {
        }
    }
}
