using Erazer.Infrastructure.MongoDb;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public interface IPositionRepository
    {
        Task<PositionDto> GetCurrentPosition();
        Task SetCurrentPosition(IMongoDbSession session, long position, DateTimeOffset timeOffset);
    }
}
