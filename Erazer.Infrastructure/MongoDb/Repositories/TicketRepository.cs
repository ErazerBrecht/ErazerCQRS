using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Domain.Infrastructure.DTOs;
using Erazer.Domain.Infrastructure.Repositories;

namespace Erazer.Infrastructure.MongoDb.Repositories
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
            var projection = Builders<TicketDto>.Projection.Expression(t => new TicketListDto { Id = t.Id, Status = t.Status, Title = t.Title, Priority = t.Priority});
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
