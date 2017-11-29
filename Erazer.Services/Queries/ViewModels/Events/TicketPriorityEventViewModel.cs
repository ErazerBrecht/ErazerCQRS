using Erazer.Services.Queries.ViewModels.Events.Enums;

namespace Erazer.Services.Queries.ViewModels.Events
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
