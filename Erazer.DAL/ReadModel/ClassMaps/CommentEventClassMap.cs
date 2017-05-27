using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.DAL.ReadModel.ClassMaps
{
    public class CommentEventClassMap : MongoDbClassMap<CommentEventDto>
    {
        public override void Map(BsonClassMap<CommentEventDto> cm)
        {
            cm.SetDiscriminator("comment");

            cm.MapField("_comment").SetElementName("comment");
        }
    }
}
