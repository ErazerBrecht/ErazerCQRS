﻿using System;

namespace Erazer.Services.Queries.DTOs.Events
{
    public abstract class TicketEventDto
    {
        public string Id { get; set; }
        public string TicketId { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
    }
}