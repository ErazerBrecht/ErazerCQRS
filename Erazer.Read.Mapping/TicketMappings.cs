using AutoMapper;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Detail;
using Erazer.Read.Data.Ticket.Events;
using Erazer.Read.ViewModels.Ticket;
using Erazer.Read.ViewModels.Ticket.Events;

namespace Erazer.Read.Mapping
{
    public class TicketMappings : Profile
    {
        public TicketMappings()
        {
            CreateMap<TicketDto, TicketViewModel>();
            CreateMap<TicketDto, TicketListViewModel>();
            CreateMap<TicketListDto, TicketListViewModel>();

            CreateMap<TicketEventDto, TicketEventViewModel>();
            CreateMap<StatusEventDto, TicketStatusEventViewModel>()
                .IncludeBase<TicketEventDto, TicketEventViewModel>();
            CreateMap<PriorityEventDto, TicketPriorityEventViewModel>()
                .IncludeBase<TicketEventDto, TicketEventViewModel>();
            CreateMap<CommentEventDto, TicketCommentEventViewModel>()
                .IncludeBase<TicketEventDto, TicketEventViewModel>();
            CreateMap<StatusEventDto, TicketStatusEventViewModel>()
                .IncludeBase<TicketEventDto, TicketEventViewModel>();
            CreateMap<CreatedEventDto, TicketCreatedEventViewModel>()
                .IncludeBase<TicketEventDto, TicketEventViewModel>();
        }
    }
}
