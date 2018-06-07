using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Data.Repositories;
using Erazer.Web.ReadAPI.ViewModels;
using MediatR;

namespace Erazer.Web.ReadAPI.Queries.Handlers
{
    public class StatusAllQueryHandler : AsyncRequestHandler<StatusAllQuery, List<StatusViewModel>>
    {
        private readonly IStatusQueryRepository _repository;
        private readonly IMapper _mapper;

        public StatusAllQueryHandler(IStatusQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected override async Task<List<StatusViewModel>> HandleCore(StatusAllQuery message)
        {
            var statuses =  await _repository.All();
            return _mapper.Map<List<StatusViewModel>>(statuses);
        }
    }
}
