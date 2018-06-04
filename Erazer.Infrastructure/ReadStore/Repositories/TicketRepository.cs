using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Domain.Data.DTOs;
using Erazer.Domain.Data.Repositories;
using Erazer.Infrastructure.MongoDb.Base;
using MongoDB.Driver;

namespace Erazer.Infrastructure.ReadStore.Repositories
{
    public class TicketRepository : MongoDbBaseRepository, ITicketQueryRepository
    {
        private readonly IMongoCollection<TicketDto> _collection;

        public TicketRepository(IMongoDatabase database) : base(database)
        {
            _collection = database.GetCollection<TicketDto>("Tickets");
        }

        public async Task<TicketDto> Find(string id)
        {
            var tickets = await _collection.FindAsync(t => t.Id == id);
            return await tickets.SingleOrDefaultAsync();
        }

        public Task<List<TicketListDto>> All()
        {
            // Create projection to only retrieve the fields we need in the READ API
            // In this case 'Description' is not showed in READ API (All tickets)
            var projection = Builders<TicketDto>.Projection.Expression(t => new TicketListDto { Id = t.Id, Status = t.Status, Title = t.Title, Priority = t.Priority, FileCount = t.Files == null ? 0 : t.Files.Count});
            return _collection.Find(t => true).Project(projection).ToListAsync();               // Find t => true means retrieve all!
        }

        public Task Update(TicketDto ticket)
        {
           return _collection.ReplaceOneAsync(t => t.Id == ticket.Id, ticket);
        }

        public Task Insert(TicketDto ticket)
        {
            return _collection.InsertOneAsync(ticket);
        }
    }
}
