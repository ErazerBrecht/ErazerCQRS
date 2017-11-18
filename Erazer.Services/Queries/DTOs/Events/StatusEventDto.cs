﻿using Erazer.Domain.Constants.Enums;

namespace Erazer.Services.Queries.DTOs.Events
{
    public class StatusEventDto : TicketEventDto
    {
        public StatusDto FromStatus { get; private set; }
        public StatusDto ToStatus { get; private set; }

        public StatusEventDto(StatusDto from, StatusDto to)
        {
            FromStatus = from;
            ToStatus = to;
        }
    }
}
