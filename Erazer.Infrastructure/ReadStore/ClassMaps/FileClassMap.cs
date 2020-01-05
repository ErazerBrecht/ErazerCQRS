using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.File;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.ReadStore.ClassMaps
{
    public class FileClassMap : MongoDbClassMap<FileDto>
    {
        public override void Map(BsonClassMap<FileDto> cm)
        {
            cm.MapIdProperty(x => x.Id);

            cm.MapProperty(f => f.Created).SetElementName("created");
            cm.MapProperty(f => f.Id).SetElementName("id");
            cm.MapProperty(f => f.Name).SetElementName("name");
            cm.MapProperty(f => f.Type).SetElementName("type");
        }
    }
}
