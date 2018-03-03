using MediatR;
using System;

namespace Erazer.Framework.Files
{
    public interface IFileUpload :  INotification
    {
        Guid Id { get; set; }

        string Name { get; set; }
        string Type { get; set; }

        Guid UserId { get; set; }
        DateTime Created { get; set; }

        byte[] Data { get; set; }
    }
}
