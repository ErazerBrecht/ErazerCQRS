using AutoMapper;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.ViewModels;

namespace Erazer.Services.Queries.Mappings
{
    public class StatusMappings : Profile
    {
        public StatusMappings()
        {
            CreateMap<StatusDto, StatusViewModel>();
        }
    }
}
