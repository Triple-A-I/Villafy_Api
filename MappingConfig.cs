using AutoMapper;
using Villafy_Api.Models;
using Villafy_Api.Models.Dto.Auth;
using Villafy_Api.Models.Dto.Villa;
using Villafy_Api.Models.Dto.VillaNumber;

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

            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDto>().ReverseMap();

            CreateMap<LocalUser, RegistrationRequestDto>().ReverseMap();

        }
    }
}
