namespace Erazer.Services.Queries.ViewModels.Events
{
    public class TicketPriorityEventViewModel: TicketEventViewModel
    {
        public PriorityViewModel ToPriority { get; set; }
        public PriorityViewModel FromPriority { get; set; }

        public TicketPriorityEventViewModel()
        {
            Type = Domain.Constants.Enums.EventType.Priority;
        }
    }
}
