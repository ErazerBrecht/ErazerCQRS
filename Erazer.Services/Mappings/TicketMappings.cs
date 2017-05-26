using AutoMapper;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.ViewModels;

namespace Erazer.Services.Mappings
{
    public class TicketMappings : Profile
    {
        public TicketMappings()
        {
            CreateMap<ITicketDto, TicketViewModel>();

            CreateMap<TicketEventDto, TicketEventViewModel>();
        }
    }
}
