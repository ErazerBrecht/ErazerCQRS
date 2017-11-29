using Erazer.Services.Queries.ViewModels.Events.Enums;

namespace Erazer.Services.Queries.ViewModels.Events
{
    public class TicketCreatedEventViewModel: TicketEventViewModel
    {
        public TicketCreatedEventViewModel()
        {
            Type = EventType.Created;
        }
    }
}
