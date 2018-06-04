using System.Threading.Tasks;
using Erazer.Domain.Files;
using Erazer.Domain.Files.Upload;
using Erazer.Infrastructure.DocumentStore;
using MediatR;

namespace Erazer.Web.DocumentStore.Command.Handlers
{
    public class UploadFileHandler : AsyncNotificationHandler<FileUpload>
    {
        private readonly IFileRepository _fileRepository;

        public UploadFileHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        protected override Task HandleCore(FileUpload message)
        {
            return _fileRepository.Save(message);
        }
    }
}
