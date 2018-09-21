using System;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;

namespace Erazer.Framework.Cache
{
    public class CacheRepository : IAggregateRepository
    {
        private readonly IAggregateRepository _repository;
        private readonly IEventStore _eventStore;
        private readonly ICache _cache;

        public CacheRepository(IAggregateRepository repository, IEventStore eventStore, ICache cache)
        {
            _repository = repository;
            _eventStore = eventStore;
            _cache = cache;
        }

        public async Task<T> Get<T>(Guid aggregateId) where T : AggregateRoot
        {
            try
            {
                T aggregate;
                if (_cache.IsTracked(aggregateId))
                {
                    aggregate = (T) _cache.Get(aggregateId);
                    var events = (await _eventStore.Get<T>(aggregateId, aggregate.Version + 1)).ToList();

                    if (events.Any() && events.First().Version != aggregate.Version + 1)
                    {
                        _cache.Remove(aggregateId);
                    }
                    else
                    {
                        aggregate.LoadFromHistory(events);
                        return aggregate;
                    }
                }

                aggregate = await _repository.Get<T>(aggregateId);
                _cache.Set(aggregateId, aggregate);
                return aggregate;
            }
            catch (Exception)
            {
                _cache.Remove(aggregateId);
                throw;
            }
        }

        // TODO Make code multi user friendly
        // If two users at the same time save an aggregate...
        public Task Save<T>(T aggregate) where T : AggregateRoot
        {
            try
            {
                // Add new aggregates or update existing aggregates!
                if (aggregate.Id != Guid.Empty) 
                {
                    _cache.Set(aggregate.Id, aggregate);
                }
                return _repository.Save(aggregate);
            }
            catch (Exception)
            {
                _cache.Remove(aggregate.Id);
                throw;
            }
        }
    }
}
