using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Domain.Data.DTOs.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.MongoDb.ClassMaps
{
    public class CommentEventClassMap : MongoDbClassMap<CommentEventDto>
    {
        public override void Map(BsonClassMap<CommentEventDto> cm)
        {
            cm.SetDiscriminator("comment");

            cm.MapProperty("Comment").SetElementName("comment");
        }
    }
}
