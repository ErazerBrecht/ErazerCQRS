using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.Ticket.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
{
    public class TicketEventClassMap : MongoDbClassMap<TicketEventDto>
    {
        public override void Map(BsonClassMap<TicketEventDto> cm)
        {
            cm.MapIdProperty(x => x.Id);

            cm.MapProperty(x => x.TicketId).SetElementName("ticketId");
            cm.MapProperty(x => x.Created).SetElementName("created");
            cm.MapProperty(x => x.UserId).SetElementName("userId");
        }
    }
}
