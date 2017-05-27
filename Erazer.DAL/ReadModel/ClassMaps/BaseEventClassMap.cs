using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.DAL.ReadModel.ClassMaps
{
    public class BaseEventClassMap : MongoDbClassMap<BaseEventDto>
    {
        public override void Map(BsonClassMap<BaseEventDto> cm)
        {

        }
    }
}
