﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Domain.Files;
using Erazer.Write.Application.Commands;
using Erazer.Write.Application.Services;
using Erazer.Write.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Write.Web.Controllers
{
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileUploader _fileUploader;

        public TicketController(IMediator mediator, IFileUploader fileUploader)
        {
            _mediator = mediator;
            _fileUploader = fileUploader;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(NewTicketViewModel model)
        {
           var files = model.Files == null || !model.Files.Any()
                ? new List<File>()
                : await _fileUploader.UploadFiles( model.Files.ToArray());

            var id = await _mediator.Send(new CreateTicketCommand
            {
                Title = model.Title,
                Description = model.Description,
                PriorityId = model.PriorityId,
                Files = files.ToList()
            });

            return Ok(id);
        }

        [HttpPatch("comment")]
        public async Task<IActionResult> AddComment([FromBody] AddTicketCommentViewModel model)
        {
            await _mediator.Send(new AddTicketCommentCommand
            {
                Comment = model.Comment,
                TicketId = model.TicketId
            });

            return Ok();
        }

        [HttpPatch("priority")]
        public async Task<IActionResult> UpdatePriority([FromBody] UpdateTicketPriorityViewModel model)
        {
            await _mediator.Send(new UpdateTicketPriorityCommand
            {
                PriorityId = model.PriorityId,
                TicketId = model.TicketId
            });

            return Ok();
        }

        [HttpPatch("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateTicketStatusViewModel model)
        {
            await _mediator.Send(new UpdateTicketStatusCommand
            {
                StatusId = model.StatusId,
                TicketId = model.TicketId
            });

            return Ok();
        }
    }
}