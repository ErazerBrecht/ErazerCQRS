using Erazer.Infrastructure.MongoDb.Base;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
{
    public class TicketCreatedEventClassMap : MongoDbClassMap<CreatedEventDto>
    {
        public override void Map(BsonClassMap<CreatedEventDto> cm)
        {
            cm.SetDiscriminator("created");
        }
    }
}
