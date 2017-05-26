using AutoMapper;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.ViewModels;

namespace Erazer.Services.Mappings
{
    public class TicketMappings : Profile
    {
        public TicketMappings()
        {
            CreateMap<TicketDto, TicketViewModel>()
                .ForMember(vm => vm.Priority,
                    opt => opt.MapFrom(dto => new PriorityViewModel {Id = dto.PriorityId.ToString(), Name = dto.PriorityName}))
                .ForMember(vm => vm.Status,
                    opt => opt.MapFrom(dto => new StatusViewModel {Id = dto.StatusId.ToString(), Name = dto.StatusName}));

            CreateMap<TicketEventDto, TicketEventViewModel>();
        }
    }
}
