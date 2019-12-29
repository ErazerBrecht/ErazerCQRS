using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Read.Application.Infrastructure;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Events;
using Erazer.Read.ViewModels.Ticket;
using Erazer.Read.ViewModels.Ticket.Events;
using MediatR;

namespace Erazer.Read.Application.Queries.Handlers
{
    public class TicketQueryHandler : IRequestHandler<TicketQuery, TicketViewModel>
    {
        private readonly IDbQuery<TicketDto> _ticketDbQuery;
        private readonly IDbQuery<TicketEventDto> _eventDbQuery;
        private readonly IMapper _mapper;

        public TicketQueryHandler(IDbQuery<TicketDto> ticketDbQuery, IDbQuery<TicketEventDto> eventDbQuery, IMapper mapper)
        {
            _ticketDbQuery = ticketDbQuery ?? throw new ArgumentNullException(nameof(ticketDbQuery));
            _eventDbQuery = eventDbQuery ?? throw new ArgumentNullException(nameof(eventDbQuery));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TicketViewModel> Handle(TicketQuery request, CancellationToken cancellationToken)
        {
            var ticketId = request.Id.ToString();
            var ticket = _ticketDbQuery.Single(x => x.Id == ticketId, cancellationToken);
            var events = _eventDbQuery.Find(x => x.TicketId == ticketId, cancellationToken);

            await Task.WhenAll(ticket, events);

            var ticketViewModel = _mapper.Map<TicketViewModel>(ticket.Result);
            ticketViewModel.Events = _mapper.Map<List<TicketEventViewModel>>(events.Result);

            return ticketViewModel;
        }
    }
}
