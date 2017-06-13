using System;

namespace Erazer.Web.WriteAPI.CommandViewModels
{
    public class AddTicketCommentViewModel
    {
        public Guid TicketId { get; set; }
        public string Comment { get; set; }
    }
}
