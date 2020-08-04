using Erazer.Framework.DTO;

namespace Erazer.Syncing.Models
{
    public class SubscriptionDto : IDto
    {
        // TODO In future we will have a subscription per 'DTO'
        // Aka TicketListSubscription, TicketSubscription
        // We will need this 'id' to difference them...
        public string Id { get; set; }

        public long CheckPoint { get; set; }
        public long UpdatedAt { get; set; }
        public SubscriptionType Type { get; set; }
    }
    
    public enum SubscriptionType
    {
        Live = 1,
        ReSync = 2
    }
}