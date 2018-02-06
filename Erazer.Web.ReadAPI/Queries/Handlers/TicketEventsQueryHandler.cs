using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Erazer.Domain.Data.Repositories;
using Erazer.Web.ReadAPI.ViewModels.Events;
using Erazer.Web.ReadAPI.Queries.Requests;

namespace Erazer.Web.ReadAPI.Queries.Handler
{
    public class TicketEventsQueryHandler : IAsyncRequestHandler<TicketEventsQuery, List<TicketEventViewModel>>
    {
        private readonly ITicketEventQueryRepository _repository;
        private readonly IMapper _mapper;

        public TicketEventsQueryHandler(ITicketEventQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TicketEventViewModel>> Handle(TicketEventsQuery message)
        {
            var ticketEvents =  await _repository.Find(message.TicketId);
            return _mapper.Map<List<TicketEventViewModel>>(ticketEvents);
        }
    }
}
