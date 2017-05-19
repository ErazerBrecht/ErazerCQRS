using System;
using System.Threading.Tasks;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Requests;
using MediatR;

namespace Erazer.Services.Queries.Handler
{
    public class TicketListQueryHandler : IAsyncRequestHandler<TicketListQuery, TicketListDto>
    {
        public Task<TicketListDto> Handle(TicketListQuery message)
        {
            throw new NotImplementedException();
        }
    }
}
