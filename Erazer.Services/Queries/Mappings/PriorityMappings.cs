using AutoMapper;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.ViewModels;

namespace Erazer.Services.Queries.Mappings
{
    public class PriorityMappings : Profile
    {
        public PriorityMappings()
        {
            CreateMap<PriorityDto, PriorityViewModel>();
        }
    }
}
