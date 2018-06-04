using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Data.Repositories;
using Erazer.Web.ReadAPI.ViewModels;
using Erazer.Web.ReadAPI.ViewModels.Events;
using MediatR;

namespace Erazer.Web.ReadAPI.Queries.Handlers
{
    public class TicketQueryHandler : AsyncRequestHandler<TicketQuery, TicketViewModel>
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

        protected override async Task<TicketViewModel> HandleCore(TicketQuery message)
        {
            if (!Guid.TryParse(message?.Id, out var id))
            {
                throw new ArgumentException($"Not a valid ticket id: {message?.Id}", nameof(message));
            }

            var ticketId = id.ToString();
            var ticket = _repository.Find(ticketId);
            var events = _eventRepository.Find(ticketId);

            await Task.WhenAll(ticket, events);

            var ticketViewModel = _mapper.Map<TicketViewModel>(ticket.Result);
            ticketViewModel.Events = _mapper.Map<List<TicketEventViewModel>>(events.Result);

            return ticketViewModel;
        }
    }
}
