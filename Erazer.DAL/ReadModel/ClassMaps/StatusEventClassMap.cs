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

            cm.MapProperty(p => p.FromStatus).SetElementName("fromStatus");
            cm.MapProperty(p => p.ToStatus).SetElementName("toStatus");
        }
    }
}
