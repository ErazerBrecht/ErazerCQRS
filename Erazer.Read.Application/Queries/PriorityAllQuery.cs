using System.Collections.Generic;
using Erazer.Read.ViewModels.Ticket;
using MediatR;

namespace Erazer.Read.Application.Queries
{
    public class PriorityAllQuery : IRequest<List<PriorityViewModel>>
    {
    }
}
