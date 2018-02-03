using Erazer.DAL.ReadModel.Base;
using Erazer.Domain.Infrastructure.DTOs.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.DAL.ReadModel.ClassMaps
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
