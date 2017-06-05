using System;

namespace Erazer.Web.Models
{
    public class AddTicketCommentViewModel
    {
        public Guid TicketId { get; set; }
        public string Comment { get; set; }
    }
}
