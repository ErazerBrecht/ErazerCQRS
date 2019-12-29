using System;
using System.Collections.Generic;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Events;

namespace Erazer.Infrastructure.ReadStore
{
    public static class CollectionNameMapping
    {
        public static Dictionary<Type, string> CollectionNames = new Dictionary<Type, string>
        {
            {typeof(PositionDto), "Position"},
            {typeof(PriorityDto), "Priorities"},
            {typeof(StatusDto), "Statuses"},
            {typeof(TicketListDto), "TicketList"},
            {typeof(TicketDto), "Tickets"},
            {typeof(TicketEventDto), "TicketEvents"},
        };
    }
}