using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Base;
using MongoDB.Driver;
using Erazer.Domain.Infrastructure.DTOs.Events;
using Erazer.Domain.Infrastructure.Repositories;

namespace Erazer.DAL.ReadModel.Repositories
{
    public class TicketEventRepository : MongoDbBaseRepository, ITicketEventQueryRepository
    {
        private readonly IMongoCollection<TicketEventDto> _collection;

        public TicketEventRepository(IMongoDatabase database) : base(database)
        {
            _collection = database.GetCollection<TicketEventDto>("TicketEvents");

        }

        public async Task<IList<TicketEventDto>> Find(string ticketId)
        {
            var events = _collection.Find(t => t.TicketId == ticketId).SortByDescending(t => t.Created);
            return await events.ToListAsync();
        }

        public Task Add(TicketEventDto ticketEvent)
        {
            return _collection.InsertOneAsync(ticketEvent);
        }
    }
}
