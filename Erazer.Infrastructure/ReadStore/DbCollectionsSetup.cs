using System;
using System.Reflection;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Infrastructure.ReadStore
{
    public static partial class DbCollectionsSetup
    {
        public static readonly Action<MongoDbDtoBuilder> ReadStoreConfiguration =
            x =>
            {
                x.AddAssembly(Assembly.GetAssembly(typeof(PositionDto)), Assembly.GetAssembly(typeof(PriorityDto)));
                x.Dto<PositionDto>(d => d.SetCollectionName("Position"));
                x.Dto<PriorityDto>(d => d.SetCollectionName("Priorities"));
                x.Dto<StatusDto>(d => d.SetCollectionName("Statuses"));
                x.Dto<TicketListDto>(d => d.SetCollectionName("TicketList"));
                x.Dto<TicketDto>(d => d.SetCollectionName("Tickets"));
                x.Dto<TicketEventDto>(d => d.SetCollectionName("TicketEvents"));
            };
    }
}