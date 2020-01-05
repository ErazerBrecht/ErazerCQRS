using System;
using Erazer.Framework.Cache;
using Erazer.Framework.Domain;
using Erazer.Infrastructure.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceStack.Redis;

namespace Erazer.Infrastructure.Redis
{
    public class RedisCache : ICache
    {
        private readonly IRedisClientsManager _manager;
        private readonly ITelemetry _telemetryClient;
        private readonly IOptions<RedisSettings> _options;

        public RedisCache(IRedisClientsManager manager, ITelemetry telemetryClient, IOptions<RedisSettings> options)
        {
            _manager = manager;
            _telemetryClient = telemetryClient;
            _options = options;
        }

        public bool IsTracked(Guid aggregateId)
        {
            var now = DateTimeOffset.Now;
            var id = aggregateId.ToString();

            using var redis = _manager.GetClient();
            var result = redis.ContainsKey(id);
            _telemetryClient.TrackDependency("DB", "Redis", $"IsTracked succeeded - AggregateId: {id} Result: {result}", now, DateTimeOffset.Now - now, true);

            return result;
        }

        public void Set(Guid aggregateId, AggregateRoot aggregate)
        {
            var now = DateTimeOffset.Now;
            var id = aggregateId.ToString();

            using var redis = _manager.GetClient();
            redis.SetValue(id, JsonConvert.SerializeObject(aggregate, aggregate.GetType(), JsonSettings.AggregateSerializer), TimeSpan.FromSeconds(_options.Value.ExpireSeconds));
            _telemetryClient.TrackDependency("DB", "Redis", $"SetValue succeeded - AggregateId: {id} AggregateType: {aggregate.GetType()}", now, DateTimeOffset.Now - now, true);
        }

        public AggregateRoot Get(Guid aggregateId)
        {
            var now = DateTimeOffset.Now;
            var id = aggregateId.ToString();

            using var redis = _manager.GetClient();
            var result = JsonConvert.DeserializeObject<AggregateRoot>(redis.GetValue(id), JsonSettings.AggregateSerializer);
            _telemetryClient.TrackDependency("DB", "Redis", $"Get succeeded - AggregateId: {id} Version: {result.Version}", now, DateTimeOffset.Now - now, true);

            return result;
        }

        public void Remove(Guid aggregateId)
        {
            var now = DateTimeOffset.Now;
            var id = aggregateId.ToString();

            using var redis = _manager.GetClient();
            redis.Remove(id);
            _telemetryClient.TrackDependency("DB", "Redis", $"Remove succeeded - AggregateId: {id}", now, DateTimeOffset.Now - now, true);
        }
    }
}
