using Erazer.Domain.Files;
using MediatR;
using System;

namespace Erazer.Web.DocumentStore.CQRS
{
    public class FileRequest: IRequest<FileUpload>
    {
        public Guid Id { get; set; }
    }
}
