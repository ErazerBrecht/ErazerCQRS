using System;
using Erazer.Framework.Files;

namespace Erazer.Domain.Files.Upload
{
    // TODO This isn't actaully a domain class
    // TODO Move this class
    public class FileUpload : IFileUpload
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
        public byte[] Data { get; set; }
    }
}
