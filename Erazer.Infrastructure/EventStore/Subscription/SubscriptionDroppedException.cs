namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class SubscriptionDroppedException : Exception
    {
        public SubscriptionDroppedException(SubscriptionDroppedReason reason, Exception inner = null): base(reason.ToString(), inner)
        {
        }
    }
}