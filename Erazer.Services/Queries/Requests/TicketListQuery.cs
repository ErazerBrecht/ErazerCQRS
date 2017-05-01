using Erazer.DAL.ReadModel;
using MediatR;

namespace Erazer.Services.Queries.Requests
{
    public class TicketListQuery : IRequest<TicketListDto>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
