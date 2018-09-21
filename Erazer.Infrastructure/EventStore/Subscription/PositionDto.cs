using System;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class PositionDto
    {
        public long CheckPoint { get; set; }
        public long UpdatedAt { get; set; }
    }
}
