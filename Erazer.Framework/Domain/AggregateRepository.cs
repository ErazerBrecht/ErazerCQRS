using System;
using System.Collections.Generic;
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
            var events = await _eventStore.Get(aggregateId, -1);
            var eventList = events as IList<IEvent> ?? events.ToList();

            if (!eventList.Any())
            {
                throw new AggregateNotFoundException(typeof(T), aggregateId);
            }

            var aggregate = AggregateFactory.Build<T>();
            aggregate.LoadFromHistory(eventList);
            return aggregate;
        }

        public Task Save<T>(T aggregate) where T : AggregateRoot
        {
            var changes = aggregate.FlushChanges();
            return _eventStore.Save(aggregate.Id, changes);
        }
    }
}
