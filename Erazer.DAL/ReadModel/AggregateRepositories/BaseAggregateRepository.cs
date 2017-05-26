using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Base;
using Erazer.Framework.Domain;
using Erazer.Framework.Domain.Repositories;
using Erazer.Framework.Events;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.ReadModel.AggregateRepositories
{
    public abstract class BaseAggregateRepository<T> : BaseRepository, IAggregateRepository<T> where T : AggregateRoot
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository _eventRepository;

        protected BaseAggregateRepository(IMediator mediator, IConfiguration configuration, IEventRepository eventRepository) : base(configuration)
        {
            _mediator = mediator;
            _eventRepository = eventRepository;
        }

        public abstract Task<T> Get(string aggregateId);

        public async Task Save(T aggregate)
        {
            foreach (var @event in aggregate.FlushChanges())
            {
                _eventRepository.AddEvent(@event);
                await _mediator.Send(@event);
            }
            await _eventRepository.Commit();
        }
    }
}
