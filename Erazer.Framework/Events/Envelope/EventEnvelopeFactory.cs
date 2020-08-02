using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;

namespace Erazer.Framework.Events.Envelope
{
    public static class EventEnvelopeFactory
    {
        private static readonly ConcurrentDictionary<Type, EventEnvelopeFactoryActivator> CachedConstructors 
            = new ConcurrentDictionary<Type, EventEnvelopeFactoryActivator>();
        
        private delegate IEventEnvelope<IEvent> EventEnvelopeFactoryActivator
            (Guid aggregateRootId, long created, long version, long position, IEvent @event);

        private static EventEnvelopeFactoryActivator CreateTypeConstructor(Type eventType)
        {
            try
            {
                var eventEnvelopeType = typeof(EventEnvelope<>).MakeGenericType(eventType);
                var ctor = eventEnvelopeType.GetConstructors().Single();

                var parameterExp = new ParameterExpression[5];
                parameterExp[0] = Expression.Parameter(typeof(Guid));     // Guid aggregateRootId
                parameterExp[1] = Expression.Parameter(typeof(long));     // long created
                parameterExp[2] = Expression.Parameter(typeof(long));     // long version
                parameterExp[3] = Expression.Parameter(typeof(long));     // long position
                parameterExp[4] = Expression.Parameter(typeof(IEvent));   // IEvent event

                var argExp = new Expression[5];
                argExp[0] = parameterExp[0];
                argExp[1] = parameterExp[1];
                argExp[2] = parameterExp[2];
                argExp[3] = parameterExp[3];
                var originalEventParameter = parameterExp[4];
                var typedEventParameter = Expression.Convert(originalEventParameter, eventType);
                argExp[4] = typedEventParameter;

                var newExpr = Expression.New(ctor, argExp);
                var func = Expression.Lambda<EventEnvelopeFactoryActivator>(newExpr, parameterExp);
                return func.Compile();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEventEnvelope<IEvent> Build(IEvent @event, Guid aggregateRootId, long created, long version,
            long position)
        {
            var eventType = @event.GetType();
            
            var ctor = CachedConstructors.GetOrAdd(eventType, _ => CreateTypeConstructor(eventType));
            return ctor(aggregateRootId, created, version, position, @event);
        }
    }
}