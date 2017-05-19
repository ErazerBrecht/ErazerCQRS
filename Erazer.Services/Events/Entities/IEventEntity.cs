using System;

namespace Erazer.Services.Events.Entities
{
    public interface IEventEntity
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        Guid UserId { get; set; }
    }
}