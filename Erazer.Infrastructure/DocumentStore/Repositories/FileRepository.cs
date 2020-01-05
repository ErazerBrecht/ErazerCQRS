using System;
using System.Threading.Tasks;
using Erazer.DocumentStore.Application.DTOs;
using Erazer.DocumentStore.Application.Infrastructure;
using MongoDB.Driver;

namespace Erazer.Infrastructure.DocumentStore.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IMongoCollection<FileContentDto> _collection;

        public FileRepository(IMongoCollection<FileContentDto> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
        
        public Task Save(FileContentDto file)
        {
            return _collection.InsertOneAsync(file);
        }

        public async Task<FileContentDto> Find(Guid id)
        {
            var dtoId = id.ToString();
            var files = await _collection.FindAsync(f => f.Id == dtoId);
            return await files.SingleOrDefaultAsync();
        }
    }
}
