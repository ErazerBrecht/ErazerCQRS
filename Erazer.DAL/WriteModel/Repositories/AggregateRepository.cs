using System;
using System.Threading.Tasks;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using Marten;
using MediatR;

namespace Erazer.DAL.WriteModel.Repositories
{
    public class AggregateRepository<T> : IAggregateRepository<T> where T : AggregateRoot, new()
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IDocumentStore _store;

        public AggregateRepository(IEventPublisher eventPublisher, IDocumentStore store)
        {
            _eventPublisher = eventPublisher;
            _store = store;
        }

        public async Task<T> Get(Guid aggregateId)
        {
            using (var session = _store.OpenSession())
            {
                return await session.Events.AggregateStreamAsync<T>(aggregateId);
            }
        }

        public async Task Save(T aggregate)
        {
            using (var session = _store.OpenSession())
            {
                var events = aggregate.FlushChanges();
                
                foreach (var @event in events)
                {
                    session.Events.Append(aggregate.Id, @event);
                    await _eventPublisher.Publish(@event);
                }

                await session.SaveChangesAsync();
            }
        }
    }
}
