using Erazer.Web.ReadAPI.ViewModels.Events.Enums;

namespace Erazer.Web.ReadAPI.ViewModels.Events
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
