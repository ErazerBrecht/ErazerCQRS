using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Erazer.DAL.ReadModel.Repositories
{
    public class PriorityRepository : MongoDbBaseRepository, IPriorityQueryRepository
    {
        private readonly IMongoCollection<PriorityDto> _collection;

        public PriorityRepository(IMongoDatabase database) : base(database)
        {
            _collection = Database.GetCollection<PriorityDto>("Priorities");
        }

        public async Task<IList<PriorityDto>> All()
        {
            var priorities = await _collection.FindAsync(_ => true);
            return await priorities.ToListAsync<PriorityDto>();
        }

        public async Task<PriorityDto> Find(string id)
        {
            var priorities = await _collection.FindAsync(p => p.Id == id);
            return await priorities.SingleOrDefaultAsync();
        }
    }
}
