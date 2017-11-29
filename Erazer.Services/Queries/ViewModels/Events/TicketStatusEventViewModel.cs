using Erazer.Services.Queries.ViewModels.Events.Enums;

namespace Erazer.Services.Queries.ViewModels.Events
{
    public class TicketStatusEventViewModel: TicketEventViewModel
    {
        public StatusViewModel ToStatus { get; set; }
        public StatusViewModel FromStatus { get; set; }

        public TicketStatusEventViewModel()
        {
            Type = EventType.Status;
        }
    }
}
