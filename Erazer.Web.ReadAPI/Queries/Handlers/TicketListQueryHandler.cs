﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Erazer.Domain.Data.Repositories;
using Erazer.Web.ReadAPI.ViewModels;
using Erazer.Web.ReadAPI.Queries.Requests;

namespace Erazer.Web.ReadAPI.Queries.Handler
{
    public class TicketListQueryHandler : AsyncRequestHandler<TicketListQuery, List<TicketListViewModel>>
    {
        private readonly ITicketQueryRepository _repository;
        private readonly IMapper _mapper;

        public TicketListQueryHandler(ITicketQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected override async Task<List<TicketListViewModel>> HandleCore(TicketListQuery message)
        {
            // TODO Add pagination
            var tickets = await _repository.All();

            return _mapper.Map<List<TicketListViewModel>>(tickets);
        }
    }
}
