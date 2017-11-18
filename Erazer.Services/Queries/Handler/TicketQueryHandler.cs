using System;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Services.Queries.Repositories;
using Erazer.Services.Queries.Requests;
using Erazer.Services.Queries.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Erazer.Services.Queries.Handler
{
    public class TicketQueryHandler : IAsyncRequestHandler<TicketQuery, TicketViewModel>
    {
        private readonly ITicketQueryRepository _repository;
        private readonly ITicketEventQueryRepository _eventRepository;
        private readonly IMapper _mapper;

        public TicketQueryHandler(ITicketQueryRepository repository, ITicketEventQueryRepository eventRepository, IMapper mapper)
        {
            _repository = repository;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<TicketViewModel> Handle(TicketQuery message)
        {
            if (!Guid.TryParse(message?.Id, out Guid id))
            {
                throw new ArgumentException($"Not a valid ticket id: {message?.Id}", nameof(message));
            }

            var ticket =  await _repository.Find(id.ToString());
            var events = await _eventRepository.Find(id.ToString());

            var ticketVM = _mapper.Map<TicketViewModel>(ticket);
            ticketVM.Events = _mapper.Map<List<TicketEventViewModel>>(events);
            return ticketVM;
        }
    }
}
