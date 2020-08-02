using System;

namespace Erazer.Write.Web.ViewModels
{
    public class UpdateTicketStatusViewModel
    {
        public Guid TicketId { get; set; }
        public string StatusId { get; set; }
    }
}