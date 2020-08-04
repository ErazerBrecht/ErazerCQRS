using System;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Framework.Exceptions;

namespace Erazer.Framework.Domain
{
    public class AggregateRepository: IAggregateRepository
    {
        private readonly IEventStore _eventStore;

        public AggregateRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<T> Get<T>(Guid aggregateId) where T : AggregateRoot
        {
            var result = (await _eventStore.Get<T>(aggregateId, 0)).ToList();

            if (!result.Any())
                throw new AggregateNotFoundException(typeof(T), aggregateId);

            var events = result.Select(x => x.Event).ToList();
            var aggregate = AggregateFactory<T>.Build();
            aggregate.LoadFromHistory(aggregateId, events);
            return aggregate;
        }

        public Task Save<T>(T aggregate) where T : AggregateRoot
        {
            var currentVersion = aggregate.Version;
            var changes = aggregate.FlushChanges();

            if (aggregate.Id == default)
                throw new AggregateIdMissingException(GetType());
            if (aggregate.Version < 0)
                throw new NoEventsException(aggregate.Id);

            return _eventStore.Save<T>(aggregate.Id, currentVersion, changes);
        }
    }
}
