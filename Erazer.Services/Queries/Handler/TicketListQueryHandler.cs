using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Services.Queries.Repositories;
using Erazer.Services.Queries.Requests;
using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Queries.Handler
{
    public class TicketListQueryHandler : IAsyncRequestHandler<TicketListQuery, List<TicketListViewModel>>
    {
        private readonly ITicketQueryRepository _repository;
        private readonly IMapper _mapper;

        public TicketListQueryHandler(ITicketQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TicketListViewModel>> Handle(TicketListQuery message)
        {
            // TODO Add pagination
            var tickets = await _repository.All();

            return _mapper.Map<List<TicketListViewModel>>(tickets);
        }
    }
}
