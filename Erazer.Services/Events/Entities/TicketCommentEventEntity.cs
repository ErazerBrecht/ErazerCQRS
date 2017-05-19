using System;
using System.ComponentModel.DataAnnotations;

namespace Erazer.Services.Events.Entities
{
    public class TicketCommentEventEntity : IEventEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
