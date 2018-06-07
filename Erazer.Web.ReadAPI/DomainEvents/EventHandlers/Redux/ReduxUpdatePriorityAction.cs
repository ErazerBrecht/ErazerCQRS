using Erazer.Framework.FrontEnd;
using Erazer.Web.ReadAPI.ViewModels.Events;

namespace Erazer.Web.ReadAPI.DomainEvents.EventHandlers.Redux
{
    public class ReduxUpdatePriorityAction : ReduxAction<TicketPriorityEventViewModel>
    {
        public ReduxUpdatePriorityAction(TicketPriorityEventViewModel payload) : base(ReduxActionTypeConstants.UpdateTicketPriority, payload)
        {
        }
    }
}
