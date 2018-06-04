using System.Threading.Tasks;
using Erazer.Domain.Files;
using Erazer.Domain.Files.Upload;
using Erazer.Infrastructure.DocumentStore;
using MediatR;

namespace Erazer.Web.DocumentStore.Query.Handlers
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
