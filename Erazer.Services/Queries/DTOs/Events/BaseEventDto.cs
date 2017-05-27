using Erazer.Domain.Constants.Enums;

namespace Erazer.Services.Queries.DTOs.Events
{
    public abstract class BaseEventDto
    {
        public abstract EventType Type { get; }

        public  abstract string From { get; }
        public abstract string To { get; }
    }
}