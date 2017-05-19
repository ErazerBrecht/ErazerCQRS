using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Services.Queries.Repositories;
using Erazer.Services.Queries.Requests;
using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Queries.Handler
{
    public class StatusAllQueryHandler : IAsyncRequestHandler<StatusAllQuery, List<StatusViewModel>>
    {
        private readonly IStatusQueryRepository _repository;
        private readonly IMapper _mapper;

        public StatusAllQueryHandler(IStatusQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<StatusViewModel>> Handle(StatusAllQuery message)
        {
            var statuses =  await _repository.All();
            return _mapper.Map<List<StatusViewModel>>(statuses);
        }
    }
}
