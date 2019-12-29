namespace Erazer.Read.ViewModels.Ticket
{
    public class TicketListViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public PriorityViewModel Priority { get; set; }
        public StatusViewModel Status { get; set; }
    }
}