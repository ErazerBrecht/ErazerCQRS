using Erazer.Framework.FrontEnd;
using Erazer.Web.ReadAPI.ViewModels.Events;

namespace Erazer.Web.ReadAPI.EventHandlers.Redux
{
    public class ReduxUpdateStatusAction : ReduxAction<TicketStatusEventViewModel>
    {
        public ReduxUpdateStatusAction(TicketStatusEventViewModel payload) : base(ReduxActionTypeConstants.UpdateTicketStatus, payload)
        {
        }
    }
}
