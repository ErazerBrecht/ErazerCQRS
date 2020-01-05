using System;
using System.Threading.Tasks;
using Erazer.DocumentStore.Application.DTOs;

namespace Erazer.DocumentStore.Application.Infrastructure
{
    public interface IFileRepository
    {
        Task<FileContentDto> Find(Guid id);
        Task Save(FileContentDto file);
    }
}