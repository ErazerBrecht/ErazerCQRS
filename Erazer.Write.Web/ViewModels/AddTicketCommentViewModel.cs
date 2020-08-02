using System;

namespace Erazer.Write.Web.ViewModels
{
    public class AddTicketCommentViewModel
    {
        public Guid TicketId { get; set; }
        public string Comment { get; set; }
    }
}
