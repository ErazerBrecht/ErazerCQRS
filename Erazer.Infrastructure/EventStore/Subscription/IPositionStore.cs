using System;
using System.Threading.Tasks;
using Erazer.Infrastructure.MongoDb;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public interface IPositionStore
    {
        Task<PositionDto> GetCurrentPosition();
        Task SetCurrentPosition(IDbSession session, long position, DateTimeOffset timeOffset);
    }
}
