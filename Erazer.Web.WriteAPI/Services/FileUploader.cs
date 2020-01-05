using Erazer.Messages.Commands;
using Erazer.Messages.Commands.Infrastructure;
using Erazer.Messages.Commands.Models;
using TicketFile = Erazer.Domain.Files.File;

namespace Erazer.Web.WriteAPI.Services
{
    public class FileUploader : IFileUploader
    {
        private readonly ICommandPublisher _publisher;

        public FileUploader(ICommandPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task<IEnumerable<TicketFile>> UploadFiles(Guid userId, params IFormFile[] formFiles)
        {
            var files = GenerateFileUploadCommands(userId, formFiles).ToList();
            await _publisher.Publish<UploadFileCommand>(files, CommandBusEndPoints.ErazerDocumentStore);

            return files.Select(f => new TicketFile(f.Id, f.Name, f.Type, f.Data.Length, f.Created, f.UserId));
        }

        private static IEnumerable<UploadFileCommand> GenerateFileUploadCommands(Guid userId, params IFormFile[] files)
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
                    Created = DateTime.UtcNow,
                    UserId = userId,
                    Name = file.FileName,
                    Type = file.ContentType,
                    Data = data
                };
            }
        }

        // TODO Move to specific ImageProcessorService
        private static byte[] CompressImage(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            using (var image = Image.Load(stream))
            using (var output = new MemoryStream())
            {
                IImageEncoder encoder;

                #region Content Checking + Compression
                switch (file.ContentType)
                {
                    case "image/jpeg":
                        encoder = new JpegEncoder { IgnoreMetadata = true, Quality = 60 };
                        break;
                    case "image/png":
                        encoder = new PngEncoder { CompressionLevel = 5 };
                        break;
                    default:
                        throw new NotSupportedException($"File with content type {file.ContentType} is not supported!");
                }
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
}
