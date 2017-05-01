using System;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.DAL.Dapper.Repositories.Base;
using Erazer.DAL.ReadModel;
using Erazer.Services.Queries.Requests;
using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Queries.Handler
{
    public class TicketQueryHandler : IAsyncRequestHandler<TicketQuery, TicketViewModel>
    {
        private readonly ITicketRepository _repository;
        private readonly IMapper _mapper;

        public TicketQueryHandler(ITicketRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TicketViewModel> Handle(TicketQuery message)
        {
            var ticket =  await _repository.Find(message.Id);
            return _mapper.Map<TicketViewModel>(ticket);
        }
    }
}
