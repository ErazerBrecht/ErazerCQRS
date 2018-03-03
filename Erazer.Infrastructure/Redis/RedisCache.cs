using System;
using Erazer.Framework.Cache;
using Erazer.Framework.Domain;
using Newtonsoft.Json;
using ServiceStack.Redis;
using Erazer.Shared;

namespace Erazer.Infrastructure.Redis
{
    public class RedisCache : ICache
    {
        private readonly IRedisClientsManager _manager;

        public RedisCache(IRedisClientsManager manager)
        {
            _manager = manager;
        }

        public bool IsTracked(Guid id)
        {
            using (var redis = _manager.GetClient())
            {
                return redis.ContainsKey(id.ToString());
            }
        }

        public void Set(Guid id, AggregateRoot aggregate)
        {
            using (var redis = _manager.GetClient())
            {
                redis.SetValue(id.ToString(), JsonConvert.SerializeObject(aggregate, aggregate.GetType(), JsonSettings.AggregateSerializer));
            }
        }

        public AggregateRoot Get(Guid id)
        {
            using (var redis = _manager.GetClient())
            {
                return JsonConvert.DeserializeObject<AggregateRoot>(redis.GetValue(id.ToString()), JsonSettings.AggregateSerializer);
            }
        }

        public void Remove(Guid id)
        {
            using (var redis = _manager.GetClient())
            {
                redis.Remove(id.ToString());
            }
        }
    }
}
