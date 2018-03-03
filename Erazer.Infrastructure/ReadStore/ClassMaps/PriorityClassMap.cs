using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Domain.Data.DTOs;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.MongoDb.ClassMaps
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
