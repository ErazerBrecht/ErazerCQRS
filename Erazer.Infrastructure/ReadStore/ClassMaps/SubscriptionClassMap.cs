using Erazer.Infrastructure.MongoDb;
using Erazer.Syncing.Models;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
{
    public class SubscriptionClassMap : MongoDbClassMap<SubscriptionDto>
    {
        public override void Map(BsonClassMap<SubscriptionDto> cm)
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        }
    }
}
