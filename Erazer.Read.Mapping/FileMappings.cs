using AutoMapper;
using Erazer.Read.Data.File;
using Erazer.Read.ViewModels.File;

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
