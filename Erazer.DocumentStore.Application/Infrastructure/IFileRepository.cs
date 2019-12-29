using System;
using System.Threading.Tasks;
using Erazer.Domain.Files.Data.DTOs;

namespace Erazer.Infrastructure.DocumentStore
{
    public interface IFileRepository
    {
        Task<FileContentDto> Find(Guid id);
        Task Save(FileContentDto file);
    }
}