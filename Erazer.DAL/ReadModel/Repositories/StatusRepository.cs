using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Erazer.DAL.ReadModel.Repositories
{
    public class StatusRepository : MongoDbBaseRepository, IStatusQueryRepository
    {
        private readonly IMongoCollection<StatusDto> _collection;

        public StatusRepository(IMongoDatabase database) : base(database)
        {
            _collection = database.GetCollection<StatusDto>("Statuses");
        }

        public async Task<IList<StatusDto>> All()
        {
            var statuses = await _collection.FindAsync(_ => true);
            return await statuses.ToListAsync();
        }

        public async Task<StatusDto> Find(string id)
        {
            var status = await _collection.FindAsync(s => s.Id == id);
            return await status.SingleOrDefaultAsync();
        }
    }
}
