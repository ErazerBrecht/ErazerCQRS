using System;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.DAL.EF.Entities;
using Erazer.DAL.EF.Repositories.Base;
using Erazer.Framework.Events;

namespace Erazer.DAL.EF.Repositories
{
    public class EventRepository : BaseRepository<TicketEventEntity>, IEventRepository
    {
        private readonly IMapper _mapper;

        public EventRepository(ErazerEventContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public void AddEvent(IEvent @event)
        {
            var entity = _mapper.Map<TicketEventEntity>(@event);
            entity.Id = Guid.NewGuid();
            this.Add(entity);
        }
    }
}
