using Erazer.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Erazer.DAL.EF
{
    public class ErazerEventContext : DbContext
    {
        public ErazerEventContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TicketEventEntity> TicketEvents { get; set; }
        public DbSet<TicketCommentEventEntity> TicketCommentEvents { get; set; }
        public DbSet<TicketPriorityEventEntity> TicketPriorityEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TicketCommentEventEntity>()
                .HasOne(e => e.TicketEvent)
                .WithOne(e => e.CommentEvent)
                .HasForeignKey<TicketCommentEventEntity>(e => e.Id);

            modelBuilder.Entity<TicketPriorityEventEntity>()
                .HasOne(e => e.TicketEvent)
                .WithOne(e => e.PriorityEvent)
                .HasForeignKey<TicketPriorityEventEntity>(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
