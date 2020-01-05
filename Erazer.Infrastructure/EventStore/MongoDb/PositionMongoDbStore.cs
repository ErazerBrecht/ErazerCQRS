using System;
using System.Threading.Tasks;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace Erazer.Infrastructure.EventStore.MongoDb
{
    public class PositionMongoDbStore: IPositionStore
    {
        private readonly IMongoCollection<PositionDto> _collection;

        public PositionMongoDbStore(IMongoCollection<PositionDto> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
        
        public Task<PositionDto> GetCurrentPosition()
        {
            return _collection.AsQueryable().SingleOrDefaultAsync();
        }

        public Task SetCurrentPosition(IDbSession session, long position, DateTimeOffset timeOffset)
        {
            var filter = new FilterDefinitionBuilder<PositionDto>().Empty;
            var update = Builders<PositionDto>.Update
                .SetOnInsert(x => x.Id, "ERAZER_CQRS_SUBSCRIPTION_POSITION")
                .Set(x => x.CheckPoint, position)
                .Set(x => x.UpdatedAt, timeOffset.ToUnixTimeSeconds());
            return _collection.UpdateOneAsync(session.Handle, filter, update, new UpdateOptions { IsUpsert = true });
        }
    }
}