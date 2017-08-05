using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using MongoDB.Driver;

namespace Erazer.DAL.ReadModel.Repositories
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

        public async Task<List<TicketListDto>> All()
        {
            // Create projection to only retrieve the fields we need in the READ API
            // In this case 'Description' is not showed in READ API (All tickets)
            var projection = Builders<TicketDto>.Projection.Expression(t => new TicketListDto { Id = t.Id, Status = t.Status, Title = t.Title, Priority = t.Priority});
            return await _collection.Find(t => true).Project(projection).ToListAsync();         // Find t => true means retrieve all!
        }

        public async Task Update(TicketDto ticket)
        {
           await _collection.ReplaceOneAsync(t => t.Id == ticket.Id, ticket);
        }

        public async Task Insert(TicketDto ticket)
        {
            await _collection.InsertOneAsync(ticket);
        }
    }
}
