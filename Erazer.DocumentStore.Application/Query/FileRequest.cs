using System;
using Erazer.Domain.Files.Data.DTOs;
using MediatR;

namespace Erazer.Web.DocumentStore.Query
{
    public class FileRequest: IRequest<FileContentDto>
    {
        public Guid Id { get; set; }
    }
}
