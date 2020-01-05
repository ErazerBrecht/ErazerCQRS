using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.Ticket;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
{
    public class StatusClassMap : MongoDbClassMap<StatusDto>
    {
        public override void Map(BsonClassMap<StatusDto> cm)
        {
            cm.MapIdProperty(x => x.Id);

            cm.MapProperty(x => x.Name)
                .SetElementName("name");
        }
    }
}
