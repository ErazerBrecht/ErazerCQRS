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

        protected AggregateRoot()
        {
            Version = -1;       
        } 

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
                Version += changes.Length;
                
                _changes.Clear();
                return changes;
            }
        }

        public void LoadFromHistory(Guid id, IEnumerable<IEvent> history)
        {
            if (Id == default)
                Id = id;
            else if (Id != id)
                throw new EventsOfWrongAggregateException(Id, id);
            
            foreach (var e in history)
            {
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
                {
                    Version++;
                }
            }
        }
    }
}
