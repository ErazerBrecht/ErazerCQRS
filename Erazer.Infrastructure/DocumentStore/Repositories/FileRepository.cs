using Erazer.Infrastructure.MongoDb.Base;
using System;
using MongoDB.Driver;
using System.Threading.Tasks;
using Erazer.Domain.Files.Data.DTOs;

namespace Erazer.Infrastructure.DocumentStore.Repositories
{
    public class FileRepository : MongoDbBaseRepository, IFileRepository
    {
        private readonly IMongoCollection<FileContentDto> _collection;

        public FileRepository(IMongoDatabase database) : base(database)
        {
            _collection = Database.GetCollection<FileContentDto>("Files");
        }

        public Task Save(FileContentDto file)
        {
            return _collection.InsertOneAsync(file);
        }

        public async Task<FileContentDto> Find(Guid id)
        {
            var files = await _collection.FindAsync(f => f.Id == id);
            return await files.SingleOrDefaultAsync();
        }
    }
}
