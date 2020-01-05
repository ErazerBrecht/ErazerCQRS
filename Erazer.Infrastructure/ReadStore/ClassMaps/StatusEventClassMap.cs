using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.Ticket.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
{
    public class StatusEventClassMap : MongoDbClassMap<StatusEventDto>
    {
        public override void Map(BsonClassMap<StatusEventDto> cm)
        {
            cm.SetDiscriminator("status");

            cm.MapProperty(p => p.FromStatus).SetElementName("fromStatus");
            cm.MapProperty(p => p.ToStatus).SetElementName("toStatus");
        }
    }
}
