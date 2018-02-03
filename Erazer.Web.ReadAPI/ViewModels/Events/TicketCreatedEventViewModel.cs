using Erazer.Web.ReadAPI.ViewModels.Events.Enums;

namespace Erazer.Web.ReadAPI.ViewModels.Events
{
    public class TicketCreatedEventViewModel: TicketEventViewModel
    {
        public TicketCreatedEventViewModel()
        {
            Type = EventType.Created;
        }
    }
}
