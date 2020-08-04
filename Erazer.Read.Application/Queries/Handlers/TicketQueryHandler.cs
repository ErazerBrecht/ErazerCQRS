using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Read.Application.Infrastructure;
using Erazer.Read.Data.Ticket.Detail;
using Erazer.Read.ViewModels.Ticket;
using MediatR;

namespace Erazer.Read.Application.Queries.Handlers
{
    public class TicketQueryHandler : IRequestHandler<TicketQuery, TicketViewModel>
    {
        private readonly IDbQuery<TicketDto> _ticketDbQuery;
        private readonly IMapper _mapper;

        public TicketQueryHandler(IDbQuery<TicketDto> ticketDbQuery, IMapper mapper)
        {
            _ticketDbQuery = ticketDbQuery ?? throw new ArgumentNullException(nameof(ticketDbQuery));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TicketViewModel> Handle(TicketQuery request, CancellationToken cancellationToken)
        {
            var ticketId = request.Id.ToString();
            var ticket = await _ticketDbQuery.Single(x => x.Id == ticketId, cancellationToken);

            var ticketViewModel = _mapper.Map<TicketViewModel>(ticket);
            return ticketViewModel;
        }
    }
}
