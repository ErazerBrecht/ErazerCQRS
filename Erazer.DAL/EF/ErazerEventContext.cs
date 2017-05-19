using Erazer.Services.Events.Entities;
using Microsoft.EntityFrameworkCore;

namespace Erazer.DAL.EF
{
    public class ErazerEventContext : DbContext
    {
        public ErazerEventContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TicketCommentEventEntity> TicketCommentEvents { get; set; }
    }
}
