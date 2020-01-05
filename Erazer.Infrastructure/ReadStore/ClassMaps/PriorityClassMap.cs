using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.Ticket;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
{
    public class PriorityClassMap : MongoDbClassMap<PriorityDto>
    {
        public override void Map(BsonClassMap<PriorityDto> cm)
        {
            cm.MapIdProperty(x => x.Id);

            cm.MapProperty(x => x.Name)
                .SetElementName("name");
        }
    }
}
