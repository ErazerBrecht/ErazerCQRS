using Erazer.Framework.DTO;

namespace Erazer.Read.Data.Ticket.List
{
    public class TicketListDto : IDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int FileCount { get; set; }

        public PriorityDto Priority { get; set; }
        public StatusDto Status { get; set; }
    }
}