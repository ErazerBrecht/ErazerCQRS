using System;

namespace Erazer.Services.Queries.DTOs
{
    public interface ITicketDto
    {
        string Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }

        IPriorityDto Priority { get; set; }
        IStatusDto Status { get; set; }
    }
}
