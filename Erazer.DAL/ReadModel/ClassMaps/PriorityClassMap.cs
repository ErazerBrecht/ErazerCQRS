using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs;
using MongoDB.Bson.Serialization;

namespace Erazer.DAL.ReadModel.ClassMaps
{
    public class PriorityClassMap : MongoDbClassMap<PriorityDto>
    {
        public override void Map(BsonClassMap<PriorityDto> cm)
        {
            cm.MapIdProperty(x => x.Id);

            cm.MapProperty(x => x.Name)
                .SetElementName("name");
        }
    }
}
