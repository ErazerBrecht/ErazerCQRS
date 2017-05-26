using System;
using Erazer.Services.Queries.DTOs;
using MongoDB.Bson.Serialization.Attributes;

namespace Erazer.DAL.ReadModel.DTOs
{
    public class PriorityDto : IPriorityDto
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
