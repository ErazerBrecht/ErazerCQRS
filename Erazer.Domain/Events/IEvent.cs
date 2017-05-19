using System;
using MediatR;

namespace Erazer.Domain.Events
{
    public interface IEvent : IRequest
    {
        Guid AggregateRootId { get; set; }
        DateTime Created { get; set; }
        Guid UserId { get; set; }
    }
}