using System.Threading.Tasks;
using MediatR;
using System;
using Erazer.Framework.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.ServiceBus
{
    public class CommandConsumer<T> : IConsumer<T> where T : class, ICommand

    {
        private readonly ILogger<CommandConsumer<T>> _logger;
        private readonly IMediator _mediator;

        public CommandConsumer(ILogger<CommandConsumer<T>> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            var command = context.Message;

            try
            {
                _logger.LogInformation($"Received '{command.GetType().Name}' command from {context.SourceAddress}");
                await _mediator.Send(command);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Exception when executing command {command}");
                throw;
            }
        }
    }
}
