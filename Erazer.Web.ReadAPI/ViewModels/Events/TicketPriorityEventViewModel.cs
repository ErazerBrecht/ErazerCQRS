using Erazer.Web.ReadAPI.ViewModels.Events.Enums;

namespace Erazer.Web.ReadAPI.ViewModels.Events
{
    public class TicketPriorityEventViewModel: TicketEventViewModel
    {
        public PriorityViewModel ToPriority { get; set; }
        public PriorityViewModel FromPriority { get; set; }

        public TicketPriorityEventViewModel()
        {
            Type = EventType.Priority;
        }
    }
}
