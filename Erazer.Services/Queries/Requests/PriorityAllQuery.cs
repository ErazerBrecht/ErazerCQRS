using System.Collections.Generic;
using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Queries.Requests
{
    public class PriorityAllQuery : IRequest<List<PriorityViewModel>>
    {
    }
}
