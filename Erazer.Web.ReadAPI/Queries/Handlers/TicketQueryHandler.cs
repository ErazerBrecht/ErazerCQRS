using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using Erazer.Domain.Data.Repositories;
using Erazer.Web.ReadAPI.ViewModels;
using Erazer.Web.ReadAPI.Queries.Requests;
using Erazer.Web.ReadAPI.ViewModels.Events;

namespace Erazer.Web.ReadAPI.Queries.Handler
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

            var ticketId = id.ToString();
            var ticket = _repository.Find(ticketId);
            var events = _eventRepository.Find(ticketId);

            await Task.WhenAll(ticket, events);

            var ticketVM = _mapper.Map<TicketViewModel>(ticket.Result);
            ticketVM.Events = _mapper.Map<List<TicketEventViewModel>>(events.Result);

            return ticketVM;
        }
    }
}
