using Erazer.Domain.Files;
using Erazer.Infrastructure.DocumentStore;
using MediatR;
using System.Threading.Tasks;

namespace Erazer.Web.WriteAPI.Commands.Handlers
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
