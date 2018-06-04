using Erazer.Domain.Data.DTOs;
using Erazer.Infrastructure.MongoDb.Base;
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
