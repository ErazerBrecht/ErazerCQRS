using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Erazer.Domain.Files;

namespace Erazer.Web.WriteAPI.Services
{
    public interface IFileUploader
    {
        Task<IEnumerable<File>> UploadFiles(Guid userId, params IFormFile[] files);
    }
}