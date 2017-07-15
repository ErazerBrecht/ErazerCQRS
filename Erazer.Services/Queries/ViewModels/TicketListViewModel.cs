namespace Erazer.Services.Queries.ViewModels
{
    public class TicketListViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public PriorityViewModel Priority { get; set; }
        public StatusViewModel Status { get; set; }
    }
}
