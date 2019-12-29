using System;

namespace Erazer.Domain.Files.Data.DTOs
{
    public class FileContentDto
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        
        // Metadata
        public string Name { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }

        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
    }
}
