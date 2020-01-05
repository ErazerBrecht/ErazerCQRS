using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.DocumentStore.Application.DTOs;
using Erazer.DocumentStore.Application.Infrastructure;
using MediatR;

namespace Erazer.DocumentStore.Application.Query.Handlers
{
    internal class RetrieveFileHandler : IRequestHandler<FileRequest, FileContentDto>
    {
        private readonly IFileRepository _fileRepository;

        public RetrieveFileHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
        }

        public Task<FileContentDto> Handle(FileRequest request, CancellationToken cancellationToken)
        {
            return _fileRepository.Find(request.Id);
        }
    }
}
