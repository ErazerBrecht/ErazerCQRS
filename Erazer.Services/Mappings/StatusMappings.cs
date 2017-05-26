using AutoMapper;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.ViewModels;

namespace Erazer.Services.Mappings
{
    public class StatusMappings : Profile
    {
        public StatusMappings()
        {
            CreateMap<IStatusDto, StatusViewModel>();
        }
    }
}
