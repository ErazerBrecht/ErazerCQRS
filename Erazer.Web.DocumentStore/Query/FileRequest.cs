using System;
using Erazer.Domain.Files;
using Erazer.Domain.Files.Upload;
using MediatR;

namespace Erazer.Web.DocumentStore.Query
{
    public class FileRequest: IRequest<FileUpload>
    {
        public Guid Id { get; set; }
    }
}
