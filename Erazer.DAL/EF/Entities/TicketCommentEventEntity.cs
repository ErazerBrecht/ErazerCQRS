using System;
using System.ComponentModel.DataAnnotations;

namespace Erazer.DAL.EF.Entities
{
    public class TicketCommentEventEntity 
    {
        public Guid Id { get; set; }
        public TicketEventEntity TicketEvent { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
