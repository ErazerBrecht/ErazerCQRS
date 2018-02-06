using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Domain.Infrastructure.DTOs;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.MongoDb.ClassMaps
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
