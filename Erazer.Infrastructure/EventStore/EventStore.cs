using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using Erazer.Framework.Events.Envelope;
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

        public async Task Save<T>(Guid aggregateId, int expectedVersion, IEnumerable<IEvent> events,
            CancellationToken cancellationToken) where T : AggregateRoot
        {
            var now = DateTimeOffset.Now;
            var domainEvents = events as List<IEvent> ?? events.ToList();

            var streamName = GetStreamName<T>(aggregateId);
            var messages = GenerateStreamMessage<T>(streamName, expectedVersion, domainEvents).ToArray();

            await _storeConnection.AppendToStream(streamName, expectedVersion, messages, cancellationToken);
            _telemetryClient.TrackDependency("DB", "EventStore (SQL)",
                $"Saving events succeeded - AggregateId: {aggregateId} EventCount: {domainEvents.Count}", now,
                DateTimeOffset.Now - now, true);
        }

        public async Task<IEnumerable<IEventEnvelope<IEvent>>> Get<T>(Guid aggregateId, int fromVersion,
            CancellationToken cancellationToken) where T : AggregateRoot
        {
            var now = DateTimeOffset.Now;
            var streamName = GetStreamName<T>(aggregateId);
            var streamMessages = new List<StreamMessage>();

            ReadStreamPage eventCollection;

            do
            {
                eventCollection =
                    await _storeConnection.ReadStreamForwards(streamName, fromVersion, 1000, cancellationToken);
                streamMessages.AddRange(eventCollection.Messages);
                fromVersion = eventCollection.NextStreamVersion;
            } while (!eventCollection.IsEnd);

            _telemetryClient.TrackDependency("DB", "EventStore (SQL)",
                $"Retrieving events succeeded - AggregateId: {aggregateId} EventCount: {streamMessages.Count}", now,
                DateTimeOffset.Now - now, true);

            return (await DeserializeMessages(streamMessages)).ToList();
        }

        public async Task<(bool IsEnd, IEnumerable<IEventEnvelope<IEvent>> EventEnvelopes)> GetAll(long fromPosition,
            int pageSize, CancellationToken cancellationToken = default)
        {
            var page = await _storeConnection.ReadAllForwards(fromPosition, pageSize, true, cancellationToken);
            var events = (await DeserializeMessages(page.Messages)).ToList();
            return (page.IsEnd, events);
        }

        public IDisposable Subscribe(long? continueAfterPosition,
            Func<IEventEnvelope<IEvent>, CancellationToken, Task> streamMessageReceived,
            Action<Exception> subscriptionDropped = null, Action<bool> hasCaughtUp = null, int? pageSize = null)
        {
            var allStreamSubscription = _storeConnection.SubscribeToAll(continueAfterPosition,
                async (subscription, message, token) =>
                {
                    var eventEnvelope = await DeserializeMessage(message);
                    await streamMessageReceived(eventEnvelope, token);
                },
                (subscription, reason, exception) =>
                {
                    subscriptionDropped?.Invoke(new SubscriptionDroppedException(reason, exception));
                },
                up => { hasCaughtUp?.Invoke(up); });

            allStreamSubscription.MaxCountPerRead = pageSize ?? 500;
            return allStreamSubscription;
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
            IEnumerable<IEvent> events) where T : AggregateRoot
        {
            var generator = new DeterministicGuidGenerator(ConstantGuid);

            foreach (var @event in events)
            {
                var jsonData = JsonConvert.SerializeObject(@event, JsonSettings.EventSerializerSettings);
                var messageId = generator.Create(streamName, expectedVersion, jsonData);
                yield return new NewStreamMessage(messageId, _eventMap.GetName(@event), jsonData);
            }
        }

        private Task<IEventEnvelope<IEvent>[]> DeserializeMessages(IEnumerable<StreamMessage> messages)
        {
            return Task.WhenAll(messages.Select(DeserializeMessage));
        }

        private async Task<IEventEnvelope<IEvent>> DeserializeMessage(StreamMessage message)
        {
            var aggregateId = GetAggregateId(message.StreamId);
            var date = new DateTimeOffset(message.CreatedUtc);
            var json = await message.GetJsonData();
            var eventType = _eventMap.GetType(message.Type);
            var @event = (IEvent) JsonConvert.DeserializeObject(json, eventType, JsonSettings.EventSerializerSettings);

            return EventEnvelopeFactory.Build(@event, aggregateId, date.ToUnixTimeMilliseconds(),
                message.StreamVersion, message.Position);
        }

        #endregion
    }
}