using System;
using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs.Events;
using MongoDB.Bson.Serialization;

namespace Erazer.DAL.ReadModel.ClassMaps
{
    public class StatusEventClassMap : MongoDbClassMap<StatusEventDto>
    {
        public override void Map(BsonClassMap<StatusEventDto> cm)
        {
            cm.SetDiscriminator("status");

            cm.MapField("_fromStatus").SetElementName("fromStatus");
            cm.MapField("_toStatus").SetElementName("toStatus");
        }
    }
}
