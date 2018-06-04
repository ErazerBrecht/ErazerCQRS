using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Data.Repositories;
using Erazer.Web.ReadAPI.ViewModels.Events;
using MediatR;

namespace Erazer.Web.ReadAPI.Queries.Handlers
{
    public class TicketEventsQueryHandler : AsyncRequestHandler<TicketEventsQuery, List<TicketEventViewModel>>
    {
        private readonly ITicketEventQueryRepository _repository;
        private readonly IMapper _mapper;

        public TicketEventsQueryHandler(ITicketEventQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected override async Task<List<TicketEventViewModel>> HandleCore(TicketEventsQuery message)
        {
            var ticketEvents =  await _repository.Find(message.TicketId);
            return _mapper.Map<List<TicketEventViewModel>>(ticketEvents);
        }
    }
}
