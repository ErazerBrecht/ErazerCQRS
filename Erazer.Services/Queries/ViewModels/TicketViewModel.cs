namespace Erazer.Services.Queries.ViewModels
{
    public class TicketViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public PriorityViewModel Priority { get; set; }
        public StatusViewModel Status { get; set; }
    }
}
