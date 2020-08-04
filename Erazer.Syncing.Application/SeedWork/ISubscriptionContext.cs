using Erazer.Syncing.Models;

namespace Erazer.Syncing.SeedWork
{
    public interface ISubscriptionContext
    {
        public SubscriptionType SubscriptionType { get; }
    }
}