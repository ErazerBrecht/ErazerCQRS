using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.DAL.Dapper.Repositories.Base;
using Erazer.Services.Queries.Requests;
using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Queries.Handler
{
    public class TicketEventsQueryHandler : IAsyncRequestHandler<TicketEventsQuery, List<TicketEventViewModel>>
    {
        private readonly ITicketEventRepository _repository;
        private readonly IMapper _mapper;

        public TicketEventsQueryHandler(ITicketEventRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TicketEventViewModel>> Handle(TicketEventsQuery message)
        {
            const int rows = 35;      // TODO Config

            if (message.Page > 1)
                message.Page = 1;

            var offset = (message.Page - 1) * rows;
            var ticketEvents =  await _repository.Find(message.TicketId, offset, rows);
            return _mapper.Map<List<TicketEventViewModel>>(ticketEvents);
        }
    }
}
