namespace Erazer.Domain.Data.DTOs.Events
{
    public class CommentEventDto : TicketEventDto
    {
        public string Comment { private get; set; }
    }
}
