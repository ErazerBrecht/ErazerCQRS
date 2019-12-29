using System;

namespace Erazer.Read.Data.File
{
    public class FileDto: IProjection

    {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Size { get; set; }

    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    }
}
