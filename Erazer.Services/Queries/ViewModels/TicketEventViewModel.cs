using System;
using Erazer.DAL.ReadModel.Enums;

namespace Erazer.Services.Queries.ViewModels
{
    public class TicketEventViewModel
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }

        public string UserName { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public EventType Type { get; set; }

    }
}
