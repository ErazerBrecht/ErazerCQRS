using System.Threading.Tasks;
using Erazer.Domain.Events;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketCommentEventHandler : IAsyncRequestHandler<TicketCommentEvent>
    {
        public TicketCommentEventHandler()
        {

        }

        public async Task Handle(TicketCommentEvent message)
        {
            // TicketCommentEvent doesn't change ReadModel for now!
        }
    }
}
