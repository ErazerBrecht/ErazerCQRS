using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Erazer.DAL.ReadModel.Base;
using Erazer.Domain.Infrastructure.DTOs;
using Erazer.Domain.Infrastructure.Repositories;


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
