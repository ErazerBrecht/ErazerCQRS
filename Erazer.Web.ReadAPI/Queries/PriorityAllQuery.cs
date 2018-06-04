using System.Collections.Generic;
using Erazer.Web.ReadAPI.ViewModels;
using MediatR;

namespace Erazer.Web.ReadAPI.Queries
{
    public class PriorityAllQuery : IRequest<List<PriorityViewModel>>
    {
    }
}
