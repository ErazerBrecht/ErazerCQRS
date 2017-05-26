using System;
using System.Collections.Generic;
using System.Text;
using Erazer.Services.Queries.DTOs;
using MongoDB.Bson.Serialization.Attributes;

namespace Erazer.DAL.ReadModel.DTOs
{
    public class TicketDto : ITicketDto
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }


        [BsonElement("priority")]
        private PriorityDto _priority;
        public IPriorityDto Priority
        {
            get { return _priority; }
            set { _priority = (PriorityDto) value; }
        }

        [BsonElement("status")]
        private StatusDto _status;
        public IStatusDto Status
        {
            get { return _status; }
            set { _status = (StatusDto)value; }
        }
    }
}
