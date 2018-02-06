using AutoMapper;
using Erazer.Domain.Data.DTOs;
using Erazer.Web.ReadAPI.ViewModels;

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
