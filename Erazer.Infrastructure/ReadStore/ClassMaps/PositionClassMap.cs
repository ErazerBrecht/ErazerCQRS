using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.MongoDb.Base;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
{
    public class PositionClassMap : MongoDbClassMap<PositionDto>
    {
        public override void Map(BsonClassMap<PositionDto> cm)
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        }
    }
}
