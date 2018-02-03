using Erazer.Framework.FrontEnd;
using Erazer.Web.ReadAPI.ViewModels;

namespace Erazer.Web.ReadAPI.EventHandlers.Redux
{
    public class ReduxTicketCreateAction : ReduxAction<TicketViewModel>
    {
        public ReduxTicketCreateAction(TicketViewModel payload) : base(ReduxActionTypeConstants.AddTicket, payload)
        {
        }
    }
}
