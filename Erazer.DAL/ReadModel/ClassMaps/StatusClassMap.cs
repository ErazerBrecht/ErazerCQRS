using Erazer.DAL.ReadModel.Base;
using Erazer.Domain.Infrastructure.DTOs;
using MongoDB.Bson.Serialization;

namespace Erazer.DAL.ReadModel.ClassMaps
{
    public class StatusClassMap : MongoDbClassMap<StatusDto>
    {
        public override void Map(BsonClassMap<StatusDto> cm)
        {
            cm.MapIdProperty(x => x.Id);

            cm.MapProperty(x => x.Name)
                .SetElementName("name");
        }
    }
}
