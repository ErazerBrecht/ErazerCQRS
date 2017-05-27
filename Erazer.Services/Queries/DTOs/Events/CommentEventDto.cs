using Erazer.Domain.Constants.Enums;

namespace Erazer.Services.Queries.DTOs.Events
{
    public class CommentEventDto : BaseEventDto
    {
        public override EventType Type => EventType.Comment;

        public override string From => null;
        public override string To => _comment;

        private string _comment;
    }
}
