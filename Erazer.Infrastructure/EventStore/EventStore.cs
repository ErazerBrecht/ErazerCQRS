using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Framework.Domain;
using Erazer.Shared;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Infrastructure;
using SqlStreamStore.Streams;

namespace Erazer.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly IStreamStore _storeConnection;
        private readonly TelemetryClient _telemetryClient;


        public EventStore(IStreamStore storeConnection, TelemetryClient telemetryClient)
        {
            _storeConnection = storeConnection;
            _telemetryClient = telemetryClient;
        }

        // TODO FIX EXPECTEDVERSION!!!
        public async Task Save<T>(Guid aggregateId, IEnumerable<IDomainEvent> events) where T : AggregateRoot
        {
            var now = DateTimeOffset.Now;
            var domainEvents = events as List<IDomainEvent> ?? events.ToList();

            var streamName = GetStreamName<T>(aggregateId);
            var messages = GenerateStreamMessage<T>(streamName, domainEvents).ToArray();

            await _storeConnection.AppendToStream(streamName, ExpectedVersion.Any, messages);
            _telemetryClient.TrackDependency("DB", "EventStore (SQL)", $"Saving events succeeded - AggregateId: {aggregateId} EventCount: {domainEvents.Count}", now, DateTimeOffset.Now - now, true);
        }

        public async Task<IEnumerable<IDomainEvent>> Get<T>(Guid aggregateId, int fromVersion) where T : AggregateRoot
        {
            var now = DateTimeOffset.Now;
            var streamName = GetStreamName<T>(aggregateId);

            // TODO FIX HARD LIMIT OF 1000 
            var eventCollection = await _storeConnection.ReadStreamForwards(streamName, fromVersion, 1000, false);
            var result = (await DeserialzeEvents(eventCollection.Messages)).ToList();

            _telemetryClient.TrackDependency("DB", "EventStore (SQL)", $"Retrieving events succeeded - AggregateId: {aggregateId} EventCount: {result.Count}", now, DateTimeOffset.Now - now, true);
            return result;

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
