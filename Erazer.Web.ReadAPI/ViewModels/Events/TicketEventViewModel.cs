using System;
using Erazer.Web.ReadAPI.ViewModels.Events.Enums;
using Erazer.Framework.FrontEnd;

namespace Erazer.Web.ReadAPI.ViewModels.Events
{
    public abstract class TicketEventViewModel: IViewModel
    {
        public string Id { get; set; }
        public string TicketId { get; set; }
        public DateTime Created { get; set; }

        // TODO Add concept of users (identity)
        public string UserName => "ErazerBrecht";
        public EventType Type { get; protected set; }
    }
}
