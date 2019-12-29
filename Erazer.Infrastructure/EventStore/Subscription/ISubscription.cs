namespace Erazer.Infrastructure.EventStore.Subscription
{
    public interface ISubscription: IDisposable
    {
        Task Connect();
    }
}
