using System.Threading.Tasks;
using MediatR;
using System;
using Erazer.Framework.Commands;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.ServiceBus
{
    public class CommandReciever<T> : IConsumer<T> where T : class, ICommand

    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<CommandReciever<T>> _logger;

        public CommandReciever(IServiceProvider provider, ILogger<CommandReciever<T>> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            // Process command
            try
            {
                using (var scope = _provider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(context.Message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Exception when executing command {context.Message}");
                throw;
            }
        }
    }
}
