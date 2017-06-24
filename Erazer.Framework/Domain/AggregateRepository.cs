using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Framework.Exceptions;
using Erazer.Framework.Factories;

namespace Erazer.Framework.Domain
{
    public class AggregateRepository<T> : IAggregateRepository<T> where T : AggregateRoot
    {
        private readonly IEventStore _eventStore;
        private readonly IFactory<T> _factory;

        public AggregateRepository(IEventStore eventStore, IFactory<T> factory)
        {
            _eventStore = eventStore;
            _factory = factory;
        }

        public async Task<T> Get(Guid aggregateId)
        {
            var events = await _eventStore.Get(aggregateId, -1);
            var eventList = events as IList<IEvent> ?? events.ToList();

            if (!eventList.Any())
            {
                throw new AggregateNotFoundException(typeof(T), aggregateId);
            }

            var aggregate = _factory.Build();
            aggregate.LoadFromHistory(eventList);
            return aggregate;
        }

        public async Task Save(T aggregate)
        {
            var changes = aggregate.FlushChanges();
            await _eventStore.Save(aggregate.Id, changes);
        }
    }
}
