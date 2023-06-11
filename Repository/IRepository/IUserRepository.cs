using Villafy_Api.Models;
using Villafy_Api.Models.Dto.Auth;

namespace Villafy_Api.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUserUnique(string username);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<LocalUser> Register(RegistrationRequestDto registrationRequestDto);
    }
}
