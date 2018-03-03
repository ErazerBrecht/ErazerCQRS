using AutoMapper;
using Erazer.Domain.Files.Data.DTOs;
using Erazer.Web.ReadAPI.ViewModels;

namespace Erazer.Services.Queries.Mappings
{
    public class FileMappings : Profile
    {
        public FileMappings()
        {
            CreateMap<FileDto, FileViewModel>();
        }
    }
}
