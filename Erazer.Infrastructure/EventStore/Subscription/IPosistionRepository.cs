using System;
using System.Threading.Tasks;
using Erazer.Infrastructure.MongoDb;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public interface IPositionRepository
    {
        Task<PositionDto> GetCurrentPosistion();
        Task SetCurrentPosition(IMongoDbSession session, long position, DateTimeOffset timeOffset);
    }
}
