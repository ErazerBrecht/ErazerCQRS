using System;
using System.Collections.Generic;
using Erazer.Framework.Events;

namespace Erazer.Framework.Domain
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }
        private readonly List<IEvent> _changes = new List<IEvent>();

        protected void ApplyChange(IEvent @event)
        {
            lock (_changes)
            {
                _changes.Add(@event);
            }
        }

        public IEnumerable<IEvent> FlushChanges()
        {
            lock (_changes)
            {
                var changes = _changes.ToArray();

                foreach (var @event in changes)
                {
                    if (@event.AggregateRootId == Guid.Empty)
                    {
                        @event.AggregateRootId = Id;
                    }

                    @event.Created = DateTime.UtcNow;
                }

                _changes.Clear();
                return changes;
            }
        }
    }
}
