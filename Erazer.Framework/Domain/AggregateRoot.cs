using System;
using System.Collections.Generic;
using Erazer.Framework.Events;
using Erazer.Framework.Exceptions;

namespace Erazer.Framework.Domain
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }
        public int Version { get; private set; }

        private readonly List<IEvent> _changes = new List<IEvent>();
        private readonly Dictionary<Type, Action<IEvent>> _eventHandlers = new Dictionary<Type, Action<IEvent>>();

        /// <summary>
        /// Register an event handler in the aggregate root.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler delegate.</param>
        protected void Handles<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            _eventHandlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }

        public IEnumerable<IEvent> FlushChanges()
        {
            lock (_changes)
            {
                var changes = _changes.ToArray();

                foreach (var @event in changes)
                {
                    @event.Created = DateTime.UtcNow;
                }

                _changes.Clear();
                return changes;
            }
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                if (e.Version != Version)
                {
                    throw new EventsOutOfOrderException(e.AggregateRootId);
                }
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        /// <summary>
        /// Applies an uncommitted event to the aggregate root.
        /// </summary>
        /// <param name="event">The event to apply.</param>
        /// <param name="isNew">Indicates if the event is a new or historic event</param>
        private void ApplyChange(IEvent @event, bool isNew)
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
