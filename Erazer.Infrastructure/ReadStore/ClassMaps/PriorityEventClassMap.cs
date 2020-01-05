using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.Ticket.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
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
