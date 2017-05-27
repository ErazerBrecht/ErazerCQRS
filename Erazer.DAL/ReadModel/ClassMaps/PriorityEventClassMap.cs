using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.DAL.ReadModel.ClassMaps
{
    public class PriorityEventClassMap : MongoDbClassMap<PriorityEventDto>
    {
        public override void Map(BsonClassMap<PriorityEventDto> cm)
        {
            cm.SetDiscriminator("priority");

            cm.MapField("_fromPriority").SetElementName("fromPriority");
            cm.MapField("_toPriority").SetElementName("toPriority");
        }
    }
}
