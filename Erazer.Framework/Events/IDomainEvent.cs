using System;
using MediatR;

namespace Erazer.Framework.Events
{
    public interface IDomainEvent : INotification
    {
        Guid AggregateRootId { get; set; }          // Setter is required by AutoMapper
        int Version { get; set; }                   // Setter is required by AutoMapper
        DateTime Created { get; set; }

        Guid? UserId { get; set; }
    }
}