using AutoMapper;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.DTOs.Events;
using Erazer.Services.Queries.ViewModels;
using Erazer.Services.Queries.ViewModels.Events;

namespace Erazer.Services.Queries.Mappings
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
