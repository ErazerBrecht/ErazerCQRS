using AutoMapper;
using Erazer.Read.Data.Ticket;
using Erazer.Read.ViewModels.Ticket;

namespace Erazer.Read.Mapping
{
    public class PriorityMappings : Profile
    {
        public PriorityMappings()
        {
            CreateMap<PriorityDto, PriorityViewModel>();
        }
    }
}
