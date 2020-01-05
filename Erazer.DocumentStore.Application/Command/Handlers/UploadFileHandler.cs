using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.DocumentStore.Application.DTOs;
using Erazer.DocumentStore.Application.Infrastructure;
using Erazer.Messages.Commands.Models;
using MediatR;

namespace Erazer.DocumentStore.Application.Command.Handlers
{
    // TODO convert this in RPC call that responds with ID of file!
    // This will abstract id generation away in 'DocumentStore' instead of 'all different µs'
    public class UploadFileHandler : IRequestHandler<UploadFileCommand>
    {
        private readonly IFileRepository _fileRepository;

        public UploadFileHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
        }

        public async Task<Unit> Handle(UploadFileCommand message, CancellationToken cancellationToken)
        {
            var dto = new FileContentDto
            {
                Created = message.Created,
                Data = message.Data,
                Id = message.Id.ToString(),
                Name = message.Name,
                Size = message.Data.Length,
                Type = message.Type,
                UserId = message.UserId
            };

            await _fileRepository.Save(dto);
            return Unit.Value;
        }
    }
}
