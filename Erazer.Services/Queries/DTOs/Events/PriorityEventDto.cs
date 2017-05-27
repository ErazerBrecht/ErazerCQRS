using Erazer.Domain.Constants.Enums;

namespace Erazer.Services.Queries.DTOs.Events
{
    public class PriorityEventDto : BaseEventDto
    {
        public override EventType Type =>EventType.Priority;
        public override string From => _fromPriority.Name;
        public override string To => _toPriority.Name;

        private PriorityDto _fromPriority;
        private PriorityDto _toPriority;

    }
}
