using System;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.DAL.Dapper.Repositories.Base;
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
            if (!Guid.TryParse(message?.Id, out Guid id))
            {
                throw new ArgumentException($"Not a valid ticket id: {message?.Id}", nameof(message));
            }

            var ticket =  await _repository.Find(id.ToString());
            return _mapper.Map<TicketViewModel>(ticket);
        }
    }
}
