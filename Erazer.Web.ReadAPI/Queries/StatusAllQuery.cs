using System.Collections.Generic;
using MediatR;
using Erazer.Web.ReadAPI.ViewModels;

namespace Erazer.Web.ReadAPI.Queries.Requests
{
    public class StatusAllQuery : IRequest<List<StatusViewModel>>
    {
    }
}
