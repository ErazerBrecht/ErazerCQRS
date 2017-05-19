using System.Threading.Tasks;
using Erazer.DAL.Dapper.Repositories.Base;
using Erazer.Domain.Aggregates;
using Erazer.Domain.Aggregates.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.Dapper.AggregateRepositories
{
    public abstract class BaseAggregateRepository<T> : BaseRepository, IAggregateRepository<T> where T : AggregateRoot
    {
        private readonly IMediator _mediator;

        protected BaseAggregateRepository(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        public abstract Task<T> Get(string aggregateId);

        public async Task Save(T aggregate)
        {
            foreach (var @event in aggregate.FlushChanges())
            {
               await _mediator.Send(@event);
            }
        }
    }
}
