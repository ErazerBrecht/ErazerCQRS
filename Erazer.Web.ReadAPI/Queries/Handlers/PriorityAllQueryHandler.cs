using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Data.Repositories;
using Erazer.Web.ReadAPI.ViewModels;
using MediatR;

namespace Erazer.Web.ReadAPI.Queries.Handlers
{
    public class PriorityAllQueryHandler : AsyncRequestHandler<PriorityAllQuery, List<PriorityViewModel>>
    {
        private readonly IPriorityQueryRepository _repository;
        private readonly IMapper _mapper;

        public PriorityAllQueryHandler(IPriorityQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected override async Task<List<PriorityViewModel>> HandleCore(PriorityAllQuery message)
        {
            var priorities =  await _repository.All();
            return _mapper.Map<List<PriorityViewModel>>(priorities);
        }
    }
}
