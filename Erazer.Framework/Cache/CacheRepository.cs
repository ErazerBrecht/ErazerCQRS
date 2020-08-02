using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
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

        private static SemaphoreSlim CreateLock(Guid _) => new SemaphoreSlim(1, 1);
        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> Locks = new ConcurrentDictionary<Guid, SemaphoreSlim>();

        public CacheRepository(IAggregateRepository repository, IEventStore eventStore, ICache cache)
        {
            _repository = repository;
            _eventStore = eventStore;
            _cache = cache;
        }

        public async Task<T> Get<T>(Guid aggregateId) where T : AggregateRoot
        {
            var @lock = Locks.GetOrAdd(aggregateId, CreateLock);
            await @lock.WaitAsync();

            try
            {
                T aggregate;
                if (_cache.IsTracked(aggregateId))
                {
                    aggregate = (T) _cache.Get(aggregateId);
                    var result = (await _eventStore.Get<T>(aggregateId, aggregate.Version + 1)).ToList();
                    
                    if (result.Any() && result.First().Version != aggregate.Version + 1)
                    {
                        _cache.Remove(aggregateId);
                    }
                    else
                    {
                        var events = result.Select(x => x.Event).ToList();
                        aggregate.LoadFromHistory(aggregateId, events);
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
            finally
            {
                @lock.Release();
            }
        }

        public async Task Save<T>(T aggregate) where T : AggregateRoot
        {
            var @lock = Locks.GetOrAdd(aggregate.Id, CreateLock);
            await @lock.WaitAsync();

            try
            {
                // Don't add aggregate when there is no Id
                // Don't add aggregate when the aggregate is just created (It's a personal opinion that I'd like to hit the EventStore at least once)
                // If the aggregate is already cached don't overwrite it. This is why the caching time shouldn't be that long!!
                if (aggregate.Id != Guid.Empty && aggregate.Version > -1 && !_cache.IsTracked(aggregate.Id)) 
                {
                    _cache.Set(aggregate.Id, aggregate);
                }
                await _repository.Save(aggregate);
            }
            catch (Exception)
            {
                _cache.Remove(aggregate.Id);
                throw;
            }
            finally
            {
                @lock.Release();
            }
        }
    }
}
