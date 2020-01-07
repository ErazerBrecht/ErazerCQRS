using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Domain.Files;
using Microsoft.AspNetCore.Http;

namespace Erazer.Web.WriteAPI.Services
{
    public interface IFileUploader
    {
        Task<IEnumerable<File>> UploadFiles(Guid userId, params IFormFile[] files);
    }
}