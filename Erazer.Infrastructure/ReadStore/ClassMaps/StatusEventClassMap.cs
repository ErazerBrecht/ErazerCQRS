using Erazer.Infrastructure.MongoDb.Base;

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
