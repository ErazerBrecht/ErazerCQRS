using System;
using Erazer.Framework.DTO;

namespace Erazer.DocumentStore.Application.DTOs
{
    public class FileContentDto: IDto
    {
        public string Id { get; set; }
        public byte[] Data { get; set; }
        
        // Metadata
        public string Name { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }

        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
    }
}
