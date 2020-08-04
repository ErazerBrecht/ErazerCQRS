using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.Ticket.Detail;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
{
    public class TicketClassMap : MongoDbClassMap<TicketDto>
    {
        public override void Map(BsonClassMap<TicketDto> cm)
        {
            cm.MapIdProperty(x => x.Id);

            cm.MapProperty(x => x.Description)
                .SetElementName("description");

            cm.MapProperty(x => x.Priority)
                .SetElementName("priority");

            cm.MapProperty(x => x.Status)
                .SetElementName("status");

            cm.MapProperty(x => x.Title)
                .SetElementName("title");

            cm.MapProperty(x => x.Events)
                .SetElementName("events");
            
            cm.MapProperty(x => x.Files)
                .SetElementName("files");
        }
    }
}
