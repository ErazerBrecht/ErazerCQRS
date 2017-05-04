using AutoMapper;
using Erazer.DAL.ReadModel;
using Erazer.Services.Queries.ViewModels;

namespace Erazer.Services.Mappings
{
    public class PriorityMappings : Profile
    {
        public PriorityMappings()
        {
            CreateMap<PriorityDto, PriorityViewModel>();
        }
    }
}
