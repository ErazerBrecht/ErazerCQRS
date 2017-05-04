using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.DAL.Dapper.Repositories.Base;
using Erazer.Services.Queries.Requests;
using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Queries.Handler
{
    public class PriorityAllQueryHandler : IAsyncRequestHandler<PriorityAllQuery, List<PriorityViewModel>>
    {
        private readonly IPriorityRepository _repository;
        private readonly IMapper _mapper;

        public PriorityAllQueryHandler(IPriorityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PriorityViewModel>> Handle(PriorityAllQuery message)
        {
            var priorities =  await _repository.All();
            return _mapper.Map<List<PriorityViewModel>>(priorities);
        }
    }
}
