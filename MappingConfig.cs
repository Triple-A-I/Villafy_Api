using AutoMapper;
using Villafy_Api.Models;
using Villafy_Api.Models.Dto;

namespace Villafy_Api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();
            ///Or Using Reverse
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        }
    }
}
