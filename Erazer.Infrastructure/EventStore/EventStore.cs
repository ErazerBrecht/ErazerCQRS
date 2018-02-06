using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Framework.Events;
using EventStore.ClientAPI;
using Erazer.Framework.Domain;

namespace Erazer.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreConnection _storeConnection;
        private readonly IMapper _mapper;

        public EventStore(IEventStoreConnection storeConnection, IMapper mapper)
        {
            _storeConnection = storeConnection;
            _mapper = mapper;
        }

        // AggregageId is already available in events => TODO Remove parameter
        public Task Save<T>(Guid aggregateId, IEnumerable<IEvent> events) where T : AggregateRoot
        {
            var streamName = GetStreamName<T>(aggregateId);
            var eventData = _mapper.Map<IEnumerable<EventData>>(events.OrderBy(e => e.Version));

            return _storeConnection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventData);
        }

        public async Task<IEnumerable<IEvent>> Get<T>(Guid aggregateId, int fromVersion) where T : AggregateRoot
        {
            var streamName = GetStreamName<T>(aggregateId);

            var startPosition = StreamPosition.Start;
            if (fromVersion > -1)
                // +1 is used to filter out the current version
                // Example Aggregate is on version 3, If I want all version starting from 3 I only need 4,5,6,... not 3!
                startPosition += fromVersion + 1;

            // TODO FIX HARD LIMIT OF 200 
            var eventCollection = await _storeConnection.ReadStreamEventsForwardAsync(streamName, startPosition, 200, false);
            return _mapper.Map<IEnumerable<IEvent>>(eventCollection.Events);
        }

        private static string GetStreamName<T>(Guid aggregateId) where T : AggregateRoot
        {
            return $"{typeof(T).Name}-{aggregateId}";
        }
    }
}
