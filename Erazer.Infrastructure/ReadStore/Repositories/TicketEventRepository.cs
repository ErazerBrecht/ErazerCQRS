using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Domain.Data.DTOs.Events;
using Erazer.Domain.Data.Repositories;
using Erazer.Infrastructure.MongoDb;
using Erazer.Infrastructure.MongoDb.Base;
using MongoDB.Driver;

namespace Erazer.Infrastructure.ReadStore.Repositories
{
    public class TicketEventRepository : MongoDbBaseRepository, ITicketEventQueryRepository
    {
        private readonly IMongoDbSession _session;
        private readonly IMongoCollection<TicketEventDto> _collection;

        public TicketEventRepository(IMongoDatabase database, IMongoDbSession session = null) : base(database)
        {
            _session = session;
            _collection = database.GetCollection<TicketEventDto>("TicketEvents");
        }

        public async Task<IList<TicketEventDto>> Find(string ticketId)
        {
            var events = _collection.Find(t => t.TicketId == ticketId).SortByDescending(t => t.Created);
            return await events.ToListAsync();
        }

        public Task Add(TicketEventDto ticketEvent)
        {
            return _session?.Handle.IsInTransaction == true
                ? _collection.InsertOneAsync(_session.Handle, ticketEvent)
                : _collection.InsertOneAsync(ticketEvent);
        }
    }
}
