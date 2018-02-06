using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Domain.Data.DTOs;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.MongoDb.ClassMaps
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
        }
    }
}
