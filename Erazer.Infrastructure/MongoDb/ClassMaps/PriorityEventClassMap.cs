using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Domain.Infrastructure.DTOs.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.MongoDb.ClassMaps
{
    public class PriorityEventClassMap : MongoDbClassMap<PriorityEventDto>
    {
        public override void Map(BsonClassMap<PriorityEventDto> cm)
        {
            cm.SetDiscriminator("priority");

            cm.MapProperty(p => p.FromPriority).SetElementName("fromPriority");
            cm.MapProperty(p => p.ToPriority).SetElementName("toPriority");
        }
    }
}
