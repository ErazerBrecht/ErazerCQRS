using System;
using MediatR;

namespace Erazer.Framework.Events
{
    public interface IDomainEvent : INotification
    {
        Guid AggregateRootId { get; set; }          // Setter is required by AutoMapper
        int Version { get; set; }                   // Setter is required by AutoMapper

        Guid UserId { get; set; }
        DateTime Created { get; set; }
    }
}