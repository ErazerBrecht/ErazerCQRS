namespace Erazer.Services.Queries.ViewModels.Events
{
    public class TicketStatusEventViewModel: TicketEventViewModel
    {
        public StatusViewModel ToStatus { get; set; }
        public StatusViewModel FromStatus { get; set; }

        public TicketStatusEventViewModel()
        {
            Type = Domain.Constants.Enums.EventType.Status;
        }
    }
}
