namespace Erazer.Read.Data.Ticket.Events
{
    public abstract class TicketEventDto
    {
        public string Id { get; set; }
        public long Created { get; set; }
    }
}