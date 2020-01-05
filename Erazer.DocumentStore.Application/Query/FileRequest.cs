using System;
using Erazer.DocumentStore.Application.DTOs;
using MediatR;

namespace Erazer.DocumentStore.Application.Query
{
    public class FileRequest: IRequest<FileContentDto>
    {
        public Guid Id { get; set; }
    }
}
