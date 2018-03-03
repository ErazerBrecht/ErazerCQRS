using Erazer.Domain.Files;
using System;
using System.Threading.Tasks;

namespace Erazer.Infrastructure.DocumentStore
{
    public interface IFileRepository
    {
        Task<FileUpload> Find(Guid id);
        Task Save(FileUpload file);
    }
}