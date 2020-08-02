using System;
using System.IO;
using System.Threading.Tasks;
using Erazer.DocumentStore.Application.Query;
using Erazer.Messages.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Web.DocumentStore
{
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/file/52666af8-9af3-4771-8d71-4f11e6c96211
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var file = await _mediator.Send(new FileRequest {Id = id});

            if (file == null)
                return NotFound();
            
            return File(file.Data, file.Type, file.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            var id = Guid.NewGuid();
            var content = memoryStream.ToArray();

            var command = new UploadFileCommand
            {
                Id = id,
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Data = content,
                Name = formFile.FileName,
                Type = formFile.ContentType,
            };

            await _mediator.Send(command);
            return Ok(id);
        }
    }
}