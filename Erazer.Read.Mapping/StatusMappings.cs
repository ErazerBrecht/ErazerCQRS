using AutoMapper;
using Erazer.Read.Data.Ticket;
using Erazer.Read.ViewModels.Ticket;

namespace Erazer.Read.Mapping
{
    public class StatusMappings : Profile
    {
        public StatusMappings()
        {
            CreateMap<StatusDto, StatusViewModel>();
        }
    }
}
