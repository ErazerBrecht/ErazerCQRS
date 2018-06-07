using AutoMapper;
using Erazer.Domain.Data.DTOs;
using Erazer.Web.ReadAPI.ViewModels;

namespace Erazer.Web.ReadAPI.Mappings
{
    public class PriorityMappings : Profile
    {
        public PriorityMappings()
        {
            CreateMap<PriorityDto, PriorityViewModel>();
        }
    }
}
