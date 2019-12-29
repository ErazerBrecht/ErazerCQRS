using System.Threading.Tasks;
using Erazer.Domain.Files.Data.DTOs;
using Erazer.Infrastructure.DocumentStore;
using Erazer.Messages.Commands.Models;
using MediatR;

namespace Erazer.Web.DocumentStore.Command.Handlers
{
    public class UploadFileHandler : AsyncRequestHandler<UploadFileCommand>
    {
        private readonly IFileRepository _fileRepository;

        public UploadFileHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        protected override Task HandleCore(UploadFileCommand message)
        {
            var dto = new FileContentDto
            {
                Created = message.Created,
                Data = message.Data,
                Id = message.Id,
                Name = message.Name,
                Size = message.Data.Length,
                Type = message.Type,
                UserId = message.UserId
            };

            return _fileRepository.Save(dto);
        }
    }
}
