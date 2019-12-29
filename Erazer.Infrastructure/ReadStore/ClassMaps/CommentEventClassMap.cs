using Erazer.Infrastructure.MongoDb.Base;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
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
