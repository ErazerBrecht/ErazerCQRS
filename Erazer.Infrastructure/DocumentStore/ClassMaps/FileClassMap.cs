using Erazer.Domain.Files.Data.DTOs;
using Erazer.Infrastructure.MongoDb.Base;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.DocumentStore.ClassMaps
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

    public class FileContentClassMap : MongoDbClassMap<FileContentDto>
    {
        public override void Map(BsonClassMap<FileContentDto> cm)
        {
            cm.MapProperty(f => f.Data).SetElementName("data");
        }
    }
}
