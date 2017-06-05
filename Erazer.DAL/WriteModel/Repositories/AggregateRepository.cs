using System;
using System.Threading.Tasks;
using Erazer.Framework.Domain;
using Marten;
using MediatR;

namespace Erazer.DAL.WriteModel.Repositories
{
    public class AggregateRepository<T> : IAggregateRepository<T> where T : AggregateRoot, new()
    {
        private readonly IMediator _mediator;
        private readonly IDocumentStore _store;

        public AggregateRepository(IMediator mediator, IDocumentStore store)
        {
            _mediator = mediator;
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
                    await _mediator.Publish(@event);
                }

                await session.SaveChangesAsync();
            }
        }
    }
}
