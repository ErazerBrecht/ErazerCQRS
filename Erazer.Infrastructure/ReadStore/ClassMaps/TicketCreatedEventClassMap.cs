using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.Ticket.Detail.Events;
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
