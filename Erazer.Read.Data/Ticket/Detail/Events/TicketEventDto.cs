namespace Erazer.Read.Data.Ticket.Detail.Events
{
    public abstract class TicketEventDto
    {
        public string Id { get; set; }
        public long Created { get; set; }
    }
}