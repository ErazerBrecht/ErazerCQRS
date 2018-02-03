namespace Erazer.Domain.Infrastructure.DTOs.Events
{
    public class CommentEventDto : TicketEventDto
    {
        public string Comment { private get; set; }
    }
}
