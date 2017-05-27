using Erazer.Domain.Constants.Enums;

namespace Erazer.Services.Queries.DTOs
{
    public class BaseEventDto
    {
        public EventType Type { get; set; }
        public string From { get; }
        public string To { get; }
    }
}