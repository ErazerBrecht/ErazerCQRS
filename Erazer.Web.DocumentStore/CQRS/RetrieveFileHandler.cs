using Erazer.Domain.Files;
using Erazer.Infrastructure.DocumentStore;
using MediatR;
using System.Threading.Tasks;

namespace Erazer.Web.DocumentStore.CQRS
{
    public class RetrieveFileHandler : AsyncRequestHandler<FileRequest, FileUpload>
    {
        private readonly IFileRepository _fileRepository;

        public RetrieveFileHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        protected override Task<FileUpload> HandleCore(FileRequest request)
        {
            return _fileRepository.Find(request.Id);
        }
    }
}
