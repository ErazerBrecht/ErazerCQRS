using Erazer.Domain.Files;
using Erazer.Infrastructure.DocumentStore;
using MediatR;
using System.Threading.Tasks;

namespace Erazer.Web.DocumentStore.CQRS
{
    public class RetrieveFileHandler : IAsyncRequestHandler<FileRequest, FileUpload>
    {
        private readonly IFileRepository _fileRepository;

        public RetrieveFileHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public Task<FileUpload> Handle(FileRequest request)
        {
            return _fileRepository.Find(request.Id);
        }
    }
}
