using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Domain.Infrastructure.DTOs.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.MongoDb.ClassMaps
{
    public class TicketCreatedEventClassMap : MongoDbClassMap<CreatedEventDto>
    {
        public override void Map(BsonClassMap<CreatedEventDto> cm)
        {
            cm.SetDiscriminator("created");
        }
    }
}
