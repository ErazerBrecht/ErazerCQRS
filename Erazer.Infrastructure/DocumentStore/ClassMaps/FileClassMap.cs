using Erazer.Domain.Files;
using Erazer.Domain.Files.Upload;
using Erazer.Infrastructure.MongoDb.Base;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.DocumentStore.ClassMaps
{
    public class FileClassMap : MongoDbClassMap<FileUpload>
    {
        public override void Map(BsonClassMap<FileUpload> cm)
        {
            cm.MapProperty(f => f.Created).SetElementName("created");
            cm.MapProperty(f => f.Id).SetElementName("id");
            cm.MapProperty(f => f.Name).SetElementName("name");
            cm.MapProperty(f => f.Type).SetElementName("type");
            cm.MapProperty(f => f.Data).SetElementName("data");
        }
    }
}
