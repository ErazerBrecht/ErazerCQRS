using Erazer.Web.DocumentStore.Query;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var file = await _mediator.Send(new FileRequest { Id = id });
            return File(file.Data, file.Type, file.Name);
        }
    }
}
