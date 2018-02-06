using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Domain.Infrastructure.DTOs;
using Erazer.Domain.Infrastructure.Repositories;

namespace Erazer.Infrastructure.MongoDb.Repositories
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
            var statuses = await _collection.FindAllAsync();
            return await statuses.ToListAsync();
        }

        public async Task<StatusDto> Find(string id)
        {
            var status = await _collection.FindAsync(s => s.Id == id);
            return await status.SingleOrDefaultAsync();
        }

        public Task<bool> Any()
        {
            return _collection.AsQueryable().AnyAsync();
        }

        public Task Add(StatusDto status)
        {
            return _collection.InsertOneAsync(status);
        }
    }
}
