namespace Erazer.Infrastructure
{
    public interface IBus
    {
        Task Start(CancellationToken cancellationToken = default(CancellationToken));
        Task Stop(CancellationToken cancellationToken = default(CancellationToken));
    }
}