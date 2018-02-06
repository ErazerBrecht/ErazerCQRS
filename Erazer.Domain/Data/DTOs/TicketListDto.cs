namespace Erazer.Domain.Data.DTOs
{
    public class TicketListDto
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public PriorityDto Priority { get; set; }
        public StatusDto Status { get; set; }
    }
}
