using System;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Base;
using Erazer.DAL.ReadModel.DTOs;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using MongoDB.Driver;

namespace Erazer.DAL.ReadModel.QueryRepositories
{
    public class TicketQueryRepository : MongoDbBaseRepository, ITicketQueryRepository
    {
        private readonly IMongoCollection<TicketDto> _collection;

        public TicketQueryRepository(IMongoDatabase database) : base(database)
        {
           _collection = database.GetCollection<TicketDto>("Tickets");
        }

        public async Task<ITicketDto> Find(string id)
        {
            var tickets = await _collection.FindAsync(t => t.Id == id);
            return await tickets.SingleOrDefaultAsync();
        }

        public Task<TicketListDto> All()
        {
            throw new NotImplementedException();
        }

        public Task Update(ITicketDto ticket)
        {
            throw new NotImplementedException();
        }
    }
}
