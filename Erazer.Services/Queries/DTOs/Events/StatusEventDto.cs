using Erazer.Domain.Constants.Enums;

namespace Erazer.Services.Queries.DTOs.Events
{
    public class StatusEventDto : BaseEventDto
    {
        public StatusEventDto(StatusDto from, StatusDto to)
        {
            _fromStatus = from;
            _toStatus = to;
        }

        public override EventType Type => EventType.Status;
        public override string From => _fromStatus.Name;
        public override string To => _toStatus.Name;

        private StatusDto _fromStatus;
        private StatusDto _toStatus;

    }
}
