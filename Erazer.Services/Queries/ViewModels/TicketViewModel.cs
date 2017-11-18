using System.Collections.Generic;

namespace Erazer.Services.Queries.ViewModels
{
    public class TicketViewModel : TicketListViewModel
    {
        public string Description { get; set; }
        public IList<TicketEventViewModel> Events { get; set; }
    }
}
 