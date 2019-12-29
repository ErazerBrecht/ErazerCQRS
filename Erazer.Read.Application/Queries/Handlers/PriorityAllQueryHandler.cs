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
    internal class PriorityAllQueryHandler : IRequestHandler<PriorityAllQuery, List<PriorityViewModel>>
    {
        private readonly IDbQuery<PriorityDto> _dbQuery;
        private readonly IMapper _mapper;

        public PriorityAllQueryHandler(IDbQuery<PriorityDto> dbQuery, IMapper mapper)
        {
            _dbQuery = dbQuery ?? throw new ArgumentNullException(nameof(dbQuery));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<PriorityViewModel>> Handle(PriorityAllQuery request, CancellationToken cancellationToken)
        {
            var priorities =  await _dbQuery.All(cancellationToken);
            return _mapper.Map<List<PriorityViewModel>>(priorities);
        }
    }
}
