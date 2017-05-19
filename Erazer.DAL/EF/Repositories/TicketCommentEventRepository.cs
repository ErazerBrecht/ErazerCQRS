using Erazer.DAL.EF.Repositories.Base;
using Erazer.Services.Events.Entities;
using Erazer.Services.Events.Repositories;

namespace Erazer.DAL.EF.Repositories
{
    public class TicketCommentEventRepository : BaseRepository<TicketCommentEventEntity>, ITicketCommentEventRepository
    {
        public TicketCommentEventRepository(ErazerEventContext context) : base(context)
        {
        }
    }
}
