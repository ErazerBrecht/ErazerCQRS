using System;
using System.Collections.Generic;
using Erazer.Framework.Events;
using Erazer.Framework.Exceptions;

namespace Erazer.Framework.Domain
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        private readonly List<IDomainEvent> _changes = new List<IDomainEvent>();
        private readonly Dictionary<Type, Action<IDomainEvent>> _eventHandlers = new Dictionary<Type, Action<IDomainEvent>>();

        /// <summary>
        /// Register an event handler in the aggregate root.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler delegate.</param>
        protected void Handles<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent
        {
            _eventHandlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }

        public IEnumerable<IDomainEvent> FlushChanges()
        {
            lock (_changes)
            {
                var changes = _changes.ToArray();

                foreach (var @event in changes)
                {
                    @event.AggregateRootId = Id;
                    @event.Created = DateTime.UtcNow;
                }

                _changes.Clear();
                return changes;
            }
        }

        public void LoadFromHistory(IEnumerable<IDomainEvent> history)
        {
            foreach (var e in history)
            {
                if (e.Version != Version + 1)
                {
                    throw new EventsOutOfOrderException(e.AggregateRootId);
                }
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(IDomainEvent @event)
        {
            ApplyChange(@event, true);
        }

        /// <summary>
        /// Applies an uncommitted event to the aggregate root.
        /// </summary>
        /// <param name="event">The event to apply.</param>
        /// <param name="isNew">Indicates if the event is a new or historic event</param>
        private void ApplyChange(IDomainEvent @event, bool isNew)
        {
            var eventType = @event.GetType();

            lock (_changes)
            {
                // Try find a registered apply method.
                if (_eventHandlers.ContainsKey(eventType))
                    _eventHandlers[eventType](@event);

                if (isNew)
                    // If its an historic event then add to uncommitted changes.
                    _changes.Add(@event);
                else
                   // Set the aggregate version to the events.
                   Version++;
            }
        }
    }
}
