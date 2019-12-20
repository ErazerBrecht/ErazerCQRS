using System.Threading.Tasks;
using Erazer.Domain.Files.Data.DTOs;
using Erazer.Infrastructure.DocumentStore;
using MediatR;

namespace Erazer.Web.DocumentStore.Query.Handlers
{
    internal class RetrieveFileHandler : AsyncRequestHandler<FileRequest, FileContentDto>
    {
        private readonly IFileRepository _fileRepository;

        public RetrieveFileHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        protected override Task<FileContentDto> HandleCore(FileRequest request)
        {
            return _fileRepository.Find(request.Id);
        }
    }
}
