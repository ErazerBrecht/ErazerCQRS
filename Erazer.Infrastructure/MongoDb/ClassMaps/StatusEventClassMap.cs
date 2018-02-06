using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Domain.Data.DTOs.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.MongoDb.ClassMaps
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
