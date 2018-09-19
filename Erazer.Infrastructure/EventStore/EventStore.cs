using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Framework.Events;
using Erazer.Framework.Domain;
using Erazer.Shared;
using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Infrastructure;
using SqlStreamStore.Streams;

namespace Erazer.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly IStreamStore _storeConnection;


        public EventStore(IStreamStore storeConnection)
        {
            _storeConnection = storeConnection;
        }

        // AggregageId is already available in events => TODO Remove parameter
        // TODO FIX EXPECTEDVERSION!!!
        public Task Save<T>(Guid aggregateId, IEnumerable<IDomainEvent> events) where T : AggregateRoot
        {
            var streamName = GetStreamName<T>(aggregateId);
            var messages = GenerateStreamMessage<T>(streamName, events).ToArray();

            return _storeConnection.AppendToStream(streamName, ExpectedVersion.Any, messages);
        }

        public async Task<IEnumerable<IDomainEvent>> Get<T>(Guid aggregateId, int fromVersion) where T : AggregateRoot
        {
            var streamName = GetStreamName<T>(aggregateId);

            //var startPosition = StreamPosition.Start;
            //if (fromVersion > -1)
            //    // +1 is used to filter out the current version
            //    // Example Aggregate is on version 3, If I want all version starting from 3 I only need 4,5,6,... not 3!
            //    startPosition += fromVersion + 1;

            // TODO FIX HARD LIMIT OF 1000 
            var eventCollection = await _storeConnection.ReadStreamForwards(streamName, fromVersion, 1000, false);
            return (await DeserialzeEvents(eventCollection.Messages)).ToList();
        }

        private static string GetStreamName<T>(Guid aggregateId) where T : AggregateRoot
        {
            return $"{typeof(T).Name}-{aggregateId}";
        }

        // TODO Using CLR type name is an antipattern -> FIX IT!
        private static IEnumerable<NewStreamMessage> GenerateStreamMessage<T>(string streamName, IEnumerable<IDomainEvent> events) where T : AggregateRoot
        {
            var generator = new DeterministicGuidGenerator(Guid.Parse("C34AEF64-CCE7-4B03-99D7-3279090B5271"));

            foreach (var @event in events)
            {
                var jsonData = JsonConvert.SerializeObject(@event, JsonSettings.DefaultSettings);
                var messageId = generator.Create(streamName, ExpectedVersion.Any, jsonData);
                yield return new NewStreamMessage(messageId, @event.GetType().Name, jsonData);
            }
        }

        private static Task<IDomainEvent[]> DeserialzeEvents(IEnumerable<StreamMessage> messages)
        {
            // Maybe use a custom JSON.NET deserializer...

            return Task.WhenAll(messages.Select(async m =>
            {
                var version = m.StreamVersion;
                var json = await m.GetJsonData();
                var @event = JsonConvert.DeserializeObject<IDomainEvent>(json, JsonSettings.DefaultSettings);
                @event.Version = version;
                return @event;
            }));
        }
    }
}
