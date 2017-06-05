using System;
using MediatR;

namespace Erazer.Framework.Events
{
    public interface IEvent : INotification
    {
        Guid AggregateRootId { get; set; }
        Guid UserId { get; set; }
        DateTime Created { get; set; }
    }
}