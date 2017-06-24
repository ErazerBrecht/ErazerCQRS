using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Web.Shared;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Erazer.Servicebus
{
    public class EventReciever : IEventReciever
    {
        private readonly IQueueClient _queueClient;
        private readonly IMediator _mediator;

        public EventReciever(IQueueClient queueClient, IMediator mediator)
        {
            _queueClient = queueClient;
            _mediator = mediator;
        }

        // Running this method on startup takes long
        // https://github.com/Azure/azure-service-bus-dotnet/issues/154
        public void RegisterEventReciever()
        {
            _queueClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    // Get actual message content
                    var messageBody = Encoding.UTF8.GetString(message.Body);

                    // Process the message 
                    await ProcessEvents(messageBody, token);

                    // Complete the message so that it is not received again.
                    // This can be done only if the queueClient is opened in ReceiveMode.PeekLock mode.
                    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
                });
        }

        private async Task ProcessEvents(string messageBody, CancellationToken token)
        {
            var @event = JsonConvert.DeserializeObject<IEvent>(messageBody, DefaultJsonSerializerSettings.DefaultSettings);
            await _mediator.Publish(@event, token);
        }
    }
}
