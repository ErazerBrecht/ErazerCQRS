using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Read.Application.Infrastructure;
using Erazer.Read.Data.Ticket;
using Erazer.Read.ViewModels.Ticket;
using MediatR;

namespace Erazer.Read.Application.Queries.Handlers
{
    internal class TicketListQueryHandler : IRequestHandler<TicketListQuery, List<TicketListViewModel>>
    {
        private readonly IDbQuery<TicketListDto> _dbQuery;
        private readonly IMapper _mapper;

        public TicketListQueryHandler(IDbQuery<TicketListDto> dbQuery, IMapper mapper)
        {
            _dbQuery = dbQuery ?? throw new ArgumentNullException(nameof(dbQuery));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TicketListViewModel>> Handle(TicketListQuery request, CancellationToken cancellationToken)
        {
            var tickets = request.PageIndex.HasValue && request.PageSize.HasValue
                    ? await _dbQuery.All(request.PageIndex.Value, request.PageSize.Value, cancellationToken)
                    : await _dbQuery.All(cancellationToken);
            
            return _mapper.Map<List<TicketListViewModel>>(tickets);
        }
    }
}
