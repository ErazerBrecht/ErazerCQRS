using System;
using System.Reflection;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Detail;
using Erazer.Read.Data.Ticket.Events;
using Erazer.Syncing.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Infrastructure.ReadStore
{
    public static partial class DbCollectionsSetup
    {
        public static readonly Action<MongoDbDtoBuilder> ReadStoreConfiguration =
            x =>
            {
                x.AddAssembly(Assembly.GetAssembly(typeof(SubscriptionDto)), Assembly.GetAssembly(typeof(PriorityDto)));
                x.Dto<SubscriptionDto>(d => d.SetCollectionName("Subscriptions"));
                x.Dto<PriorityDto>(d => d.SetCollectionName("Priorities"));
                x.Dto<StatusDto>(d => d.SetCollectionName("Statuses"));
                x.Dto<TicketListDto>(d => d.SetCollectionName("TicketList"));
                x.Dto<TicketDto>(d => d.SetCollectionName("Tickets"));
            };
    }
}