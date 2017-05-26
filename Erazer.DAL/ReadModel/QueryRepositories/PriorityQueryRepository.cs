using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Base;
using Erazer.DAL.ReadModel.DTOs;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using MongoDB.Driver;

namespace Erazer.DAL.ReadModel.QueryRepositories
{
    public class PriorityQueryRepository : MongoDbBaseRepository, IPriorityQueryRepository
    {
        private readonly IMongoCollection<PriorityDto> _collection;

        public PriorityQueryRepository(IMongoDatabase database) : base(database)
        {
            _collection = Database.GetCollection<PriorityDto>("Priorities");
        }

        public async Task<IList<IPriorityDto>> All()
        {
            var priorities = await _collection.FindAsync(_ => true);
            return await priorities.ToListAsync<IPriorityDto>();
        }
    }
}
