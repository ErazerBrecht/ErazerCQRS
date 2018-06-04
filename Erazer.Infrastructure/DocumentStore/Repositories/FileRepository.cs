using Erazer.Infrastructure.MongoDb.Base;
using System;
using MongoDB.Driver;
using System.Threading.Tasks;
using Erazer.Domain.Files;
using Erazer.Domain.Files.Upload;

namespace Erazer.Infrastructure.DocumentStore.Repositories
{
    public class FileRepository : MongoDbBaseRepository, IFileRepository
    {
        private readonly IMongoCollection<FileUpload> _collection;

        public FileRepository(IMongoDatabase database) : base(database)
        {
            _collection = Database.GetCollection<FileUpload>("Files");
        }

        public Task Save(FileUpload file)
        {
            return _collection.InsertOneAsync(file);
        }

        public async Task<FileUpload> Find(Guid id)
        {
            var files = await _collection.FindAsync(f => f.Id == id);
            return await files.SingleOrDefaultAsync();
        }
    }
}
