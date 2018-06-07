using System.Threading.Tasks;
using MediatR;
using System;
using MassTransit;
using Microsoft.Extensions.Logging;
using Erazer.Messages.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Infrastructure.ServiceBus
{
    public class EventReciever<T> : IConsumer<T> where T : class, IIntegrationEvent
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<EventReciever<T>> _logger;

        public EventReciever(ILogger<EventReciever<T>> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            // Process event
            try
            {
                using (var scope = _provider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Publish(context.Message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Exception when executing event {context.Message}");
                throw;
            }
        }
    }
}
