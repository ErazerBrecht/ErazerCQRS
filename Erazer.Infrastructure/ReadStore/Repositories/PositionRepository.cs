using System;
using System.Threading.Tasks;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.MongoDb;
using Erazer.Infrastructure.MongoDb.Base;
using MongoDB.Driver;

namespace Erazer.Infrastructure.ReadStore.Repositories
{
    public class PositionRepository : MongoDbBaseRepository, IPositionRepository
    {
        private readonly IMongoCollection<PositionDto> _collection;

        public PositionRepository(IMongoDatabase database) : base(database)
        {
            _collection = database.GetCollection<PositionDto>("Position");
        }

        public Task<PositionDto> GetCurrentPosition()
        {
            return _collection.AsQueryable().SingleOrDefaultAsync();
        }

        public Task SetCurrentPosition(IMongoDbSession session, long position, DateTimeOffset timeOffset)
        {
            var filter = new FilterDefinitionBuilder<PositionDto>().Empty;
            var update = Builders<PositionDto>.Update
                .Set(x => x.CheckPoint, position)
                .Set(x => x.UpdatedAt, timeOffset.ToUnixTimeSeconds());
            return _collection.UpdateOneAsync(session.Handle, filter, update, new UpdateOptions { IsUpsert = true });
        }
    }
}
