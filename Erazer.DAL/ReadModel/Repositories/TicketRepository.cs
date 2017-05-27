using System;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using MongoDB.Bson.Serialization;
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

        public Task<TicketListDto> All()
        {
            throw new NotImplementedException();
        }

        public Task Update(TicketDto ticket)
        {
            throw new NotImplementedException();
        }
    }
}
