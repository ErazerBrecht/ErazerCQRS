using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Erazer.Shared;
using System;

namespace Erazer.Infrastructure.ServiceBus
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

        public void RegisterEventReciever()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False value below indicates the Complete will be handled by the User Callback as seen in `ProcessMessagesAsync`.
                AutoComplete = false
            };

            // Register the function that will process messages
            _queueClient.RegisterMessageHandler(ProcessEvents, messageHandlerOptions);
        }

        // Use this Handler to look at the exceptions received on the MessagePump
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            throw exceptionReceivedEventArgs.Exception;
        }

        private async Task ProcessEvents(Message message, CancellationToken token)
        {
            // Deserialze event
            var messageBody = Encoding.UTF8.GetString(message.Body);
            var @event = JsonConvert.DeserializeObject<INotification>(messageBody, JsonSettings.DefaultSettings);

            // Process event
            try
            {
                await _mediator.Publish(@event, token);

                // Complete the message so that it is not received again.
                // This can be done only if the queueClient is opened in ReceiveMode.PeekLock mode (which is default).
                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch(Exception e)
            {
                // TODO LOG 
                throw;
            }
        }
    }
}
