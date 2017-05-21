using System;
using System.ComponentModel.DataAnnotations;
using Erazer.Domain.Constants.Enums;

namespace Erazer.DAL.EF.Entities
{
    public class TicketEventEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public EventType Type { get; set; }
        public TicketCommentEventEntity CommentEvent { get; set; }
        public TicketPriorityEventEntity PriorityEvent { get; set; }
    }
}
