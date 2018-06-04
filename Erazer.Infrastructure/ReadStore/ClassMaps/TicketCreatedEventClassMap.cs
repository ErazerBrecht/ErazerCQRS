using Erazer.Domain.Data.DTOs.Events;
using Erazer.Infrastructure.MongoDb.Base;
using MongoDB.Bson.Serialization;

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
