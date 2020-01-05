using Erazer.Framework.DTO;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class PositionDto : IDto
    {
        // TODO In future we will have a subscription per 'DTO'
        // Aka TicketListSubscription, TicketSubscription
        // We will need this 'id' to different them...
        public string Id { get; set; }

        public long CheckPoint { get; set; }
        public long UpdatedAt { get; set; }
    }
}