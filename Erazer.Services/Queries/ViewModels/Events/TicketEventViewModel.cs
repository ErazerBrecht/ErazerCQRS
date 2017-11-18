using System;
using Erazer.Domain.Constants.Enums;

namespace Erazer.Services.Queries.ViewModels
{
    public abstract class TicketEventViewModel
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }

        // TODO Add concept of users (identity)
        public string UserName => "ErazerBrecht";
        public EventType Type { get; protected set; }
    }
}
