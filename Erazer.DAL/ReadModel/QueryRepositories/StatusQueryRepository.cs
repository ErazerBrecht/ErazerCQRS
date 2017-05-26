using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Base;
using Erazer.DAL.ReadModel.DTOs;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using MongoDB.Driver;

namespace Erazer.DAL.ReadModel.QueryRepositories
{
    public class StatusQueryRepository : MongoDbBaseRepository, IStatusQueryRepository
    {
        private readonly IMongoCollection<StatusDto> _collection;


        public StatusQueryRepository(IMongoDatabase database) : base(database)
        {
            _collection = database.GetCollection<StatusDto>("Statuses");
        }

        public async Task<IList<IStatusDto>> All()
        {
            var statuses = await _collection.FindAsync(_ => true);
            return await statuses.ToListAsync<IStatusDto>();
        }
    }
}
