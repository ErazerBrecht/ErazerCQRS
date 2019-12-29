using AutoMapper;
using Erazer.Read.Data.File;
using Erazer.Web.ReadAPI.ViewModels;

namespace Erazer.Read.Mapping
{
    public class FileMappings : Profile
    {
        public FileMappings()
        {
            CreateMap<FileDto, FileViewModel>();
        }
    }
}
