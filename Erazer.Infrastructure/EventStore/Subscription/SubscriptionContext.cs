using Erazer.Syncing.Models;
using Erazer.Syncing.SeedWork;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    internal class SubscriptionContext : ISubscriptionContext
    {
        public SubscriptionType SubscriptionType { get; private set; }

        public SubscriptionContext()
        {
        }

        internal void InitializeContext(SubscriptionType subscriptionType)
        {
            SubscriptionType = subscriptionType;
        }
    }
}