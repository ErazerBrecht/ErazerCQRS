using System;

namespace Erazer.Web.Models
{
    public class UpdateTicketPriorityViewModel
    {
        public Guid TicketId { get; set; }
        public string PriorityId { get; set; }
    }
}