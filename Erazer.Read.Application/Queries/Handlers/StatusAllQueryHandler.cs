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
    internal class StatusAllQueryHandler : IRequestHandler<StatusAllQuery, List<StatusViewModel>>
    {
        private readonly IDbQuery<StatusDto> _dbQuery;
        private readonly IMapper _mapper;

        public StatusAllQueryHandler(IDbQuery<StatusDto> dbQuery, IMapper mapper)
        {
            _dbQuery = dbQuery ?? throw new ArgumentNullException(nameof(dbQuery));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<StatusViewModel>> Handle(StatusAllQuery request, CancellationToken cancellationToken)
        {
            var statuses =  await _dbQuery.All();
            return _mapper.Map<List<StatusViewModel>>(statuses);
        }
    }
}
