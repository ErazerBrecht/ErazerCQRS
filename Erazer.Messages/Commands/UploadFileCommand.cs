using System;
using Erazer.Framework.Commands;
using MediatR;

namespace Erazer.Messages.Commands
{
    public class UploadFileCommand: ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
        public byte[] Data { get; set; }
    }
}
