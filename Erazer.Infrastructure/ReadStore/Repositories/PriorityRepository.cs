using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Domain.Data.DTOs;
using Erazer.Domain.Data.Repositories;
using Erazer.Infrastructure.MongoDb.Base;
using MongoDB.Driver;

namespace Erazer.Infrastructure.ReadStore.Repositories
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
            return await priorities.ToListAsync();
        }

        public async Task<PriorityDto> Find(string id)
        {
            var priorities = await _collection.FindAsync(p => p.Id == id);
            return await priorities.SingleOrDefaultAsync();
        }

        public Task<bool> Any()
        {
            return _collection.AsQueryable().AnyAsync();
        }

        public Task Add(PriorityDto priority)
        {
            return _collection.InsertOneAsync(priority);
        }
    }
}
