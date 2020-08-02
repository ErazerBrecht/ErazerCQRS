using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Messages.Commands;
using Erazer.Messages.Commands.Infrastructure;
using Erazer.Messages.Commands.Models;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using File = Erazer.Domain.Files.File;

namespace Erazer.Write.Application.Services
{
    public interface IFileUploader
    {
        Task<IEnumerable<File>> UploadFiles(params IFormFile[] files);
    }
    
    public class FileUploader : IFileUploader
    {
        private readonly ICommandPublisher _publisher;

        public FileUploader(ICommandPublisher publisher)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public async Task<IEnumerable<File>> UploadFiles(params IFormFile[] formFiles)
        {
            var files = GenerateFileUploadCommands(formFiles).ToList();
            await _publisher.Publish(files, CommandBusEndPoints.ErazerDocumentStore);

            return files.Select(f => new File(f.Id, f.Name, f.Type, f.Data.Length, f.Created));
        }

        private static IEnumerable<UploadFileCommand> GenerateFileUploadCommands(params IFormFile[] files)
        {
            foreach (var file in files)
            {
                if (file.Length > 16777216)
                    throw new ArgumentOutOfRangeException(nameof(files), $"Filesize of the file {file.Name} is to big");

                // TODO Support PDF
                var data = CompressImage(file);

                yield return new UploadFileCommand
                {
                    Id = Guid.NewGuid(),
                    Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    Name = file.FileName,
                    Type = file.ContentType,
                    Data = data
                };
            }
        }

        // TODO Move to specific ImageProcessorService
        private static byte[] CompressImage(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var image = Image.Load(stream);
            using var output = new MemoryStream();

            #region Content Checking + Compression

            IImageEncoder encoder = file.ContentType switch
            {
                "image/jpeg" => new JpegEncoder {Quality = 60},
                "image/png" => new PngEncoder {CompressionLevel = PngCompressionLevel.Level7},
                _ => throw new NotSupportedException($"File with content type {file.ContentType} is not supported!")
            };

            #endregion

            #region Resizing
            if (image.Width > image.Height && image.Width > 1920)
            {
                const int width = 1920;
                var height = (int)(image.Height / (image.Width / 1920.0));
                image.Mutate(i => i.Resize(width, height));
            }
            else if (image.Height > 1080)
            {
                const int height = 1080;
                var width = (int)(image.Width / (image.Height / 1080.0));
                image.Mutate(i => i.Resize(width, height));
            }
            #endregion

            image.Save(output, encoder);
            return output.ToArray();
        }
    }
}
