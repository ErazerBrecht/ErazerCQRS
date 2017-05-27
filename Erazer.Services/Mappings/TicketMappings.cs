using AutoMapper;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.ViewModels;

namespace Erazer.Services.Mappings
{
    public class TicketMappings : Profile
    {
        public TicketMappings()
        {
            CreateMap<TicketDto, TicketViewModel>();

            CreateMap<TicketEventDto, TicketEventViewModel>()
                .ForMember(vm => vm.From, opt => opt.MapFrom(dto => dto.Event.From))
                .ForMember(vm => vm.To, opt => opt.MapFrom(dto => dto.Event.To))
                .ForMember(vm => vm.Type, opt => opt.MapFrom(dto => dto.Event.Type));
        }
    }
}
