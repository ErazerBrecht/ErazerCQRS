using Erazer.DocumentStore.Application.DTOs;
using Erazer.Infrastructure.MongoDb;
using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.DocumentStore.ClassMaps
{
    public class FileContentClassMap : MongoDbClassMap<FileContentDto>
    {
        public override void Map(BsonClassMap<FileContentDto> cm)
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        }
    }
}