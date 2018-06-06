using System;

namespace Erazer.Domain.Files.Data.DTOs
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }

        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
    }
}
