using AutoMapper;
using Erazer.Domain.Infrastructure.DTOs;
using Erazer.Web.ReadAPI.ViewModels;

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
