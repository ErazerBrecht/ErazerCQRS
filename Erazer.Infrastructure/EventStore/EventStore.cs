using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Framework.Domain;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.Logging;
using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Infrastructure;
using SqlStreamStore.Streams;

namespace Erazer.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly IStreamStore _storeConnection;
        private readonly ITelemetry _telemetryClient;
        private readonly IEventTypeMapping _eventMap;

        private static readonly Guid ConstantGuid = Guid.Parse("C34AEF64-CCE7-4B03-99D7-3279090B5271");

        public EventStore(IStreamStore storeConnection, ITelemetry telemetryClient, IEventTypeMapping eventMap)
        {
            _storeConnection = storeConnection ?? throw new ArgumentNullException(nameof(storeConnection));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _eventMap = eventMap ?? throw new ArgumentNullException(nameof(eventMap));
        }

        public async Task Save<T>(Guid aggregateId, int expectedVersion, IEnumerable<IDomainEvent> events,
            CancellationToken cancellationToken) where T : AggregateRoot
        {
            var now = DateTimeOffset.Now;
            var domainEvents = events as List<IDomainEvent> ?? events.ToList();

            var streamName = GetStreamName<T>(aggregateId);
            var messages = GenerateStreamMessage<T>(streamName, expectedVersion, domainEvents).ToArray();

            await _storeConnection.AppendToStream(streamName, expectedVersion, messages, cancellationToken);
            _telemetryClient.TrackDependency("DB", "EventStore (SQL)",
                $"Saving events succeeded - AggregateId: {aggregateId} EventCount: {domainEvents.Count}", now,
                DateTimeOffset.Now - now, true);
        }

        public async Task<IEnumerable<IDomainEvent>> Get<T>(Guid aggregateId, int fromVersion,
            CancellationToken cancellationToken) where T : AggregateRoot
        {
            var now = DateTimeOffset.Now;
            var streamName = GetStreamName<T>(aggregateId);
            var streamEvents = new List<StreamMessage>();

            ReadStreamPage eventCollection;

            do
            {
                eventCollection = await _storeConnection.ReadStreamForwards(streamName, fromVersion, 1000, cancellationToken);
                streamEvents.AddRange(eventCollection.Messages);
                fromVersion = eventCollection.NextStreamVersion;
            } while (!eventCollection.IsEnd);

            _telemetryClient.TrackDependency("DB", "EventStore (SQL)",
                $"Retrieving events succeeded - AggregateId: {aggregateId} EventCount: {streamEvents.Count}", now,
                DateTimeOffset.Now - now, true);

            return (await DeserializeEvents(streamEvents)).ToList();
        }

        public IDisposable Subscribe(long? continueAfterPosition,
            Func<long, IDomainEvent, CancellationToken, Task> streamMessageReceived,
            Action<Exception> subscriptionDropped = null)
        {
            return _storeConnection.SubscribeToAll(continueAfterPosition, async (subscription, message, token) =>
                {
                    var @event = await DeserializeEvent(message);
                    await streamMessageReceived(message.Position, @event, token);
                },
                (subscription, reason, exception) =>
                {
                    subscriptionDropped(new SubscriptionDroppedException(reason, exception));
                });
        }

        #region Helpers

        private static string GetStreamName<T>(Guid aggregateId) where T : AggregateRoot
        {
            return $"{typeof(T).Name}/{aggregateId}";
        }

        private static Guid GetAggregateId(string streamName)
        {
            var split = streamName.Split('/');
            return Guid.Parse(split[1]);
        }

        private IEnumerable<NewStreamMessage> GenerateStreamMessage<T>(string streamName, int expectedVersion,
            IEnumerable<IDomainEvent> events) where T : AggregateRoot
        {
            var generator = new DeterministicGuidGenerator(ConstantGuid);

            foreach (var @event in events)
            {
                var jsonData = JsonConvert.SerializeObject(@event, JsonSettings.EventSerializerSettings);
                var messageId = generator.Create(streamName, expectedVersion, jsonData);
                yield return new NewStreamMessage(messageId, _eventMap.GetName(@event), jsonData);
            }
        }

        private Task<IDomainEvent[]> DeserializeEvents(IEnumerable<StreamMessage> messages)
        {
            return Task.WhenAll(messages.Select(async m => await DeserializeEvent(m)));
        }

        private async Task<IDomainEvent> DeserializeEvent(StreamMessage message)
        {
            var json = await message.GetJsonData();
            var eventType = _eventMap.GetType(message.Type);
            var @event = (IDomainEvent) JsonConvert.DeserializeObject(json, eventType, JsonSettings.EventSerializerSettings);
            // TODO remove setters from 'event'
            @event.AggregateRootId = GetAggregateId(message.StreamId);
            @event.Created = message.CreatedUtc;
            @event.Version = message.StreamVersion;
            return @event;
        }

        #endregion
    }
}