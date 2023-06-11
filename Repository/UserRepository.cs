using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Villafy_Api.Data;
using Villafy_Api.Models;
using Villafy_Api.Models.Dto.Auth;
using Villafy_Api.Repository.IRepository;

namespace Villafy_Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private string _secretKey;

        public UserRepository(ApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUserUnique(string username)
        {
            var user = _dbContext.LocalUsers.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _dbContext.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName
            && u.Password == loginRequestDto.Password);
            if (user == null)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    LocalUser = null
                };

            }

            //TODO: If user was found generate token

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                LocalUser = user
            };

            return loginResponseDto;
        }

        public async Task<LocalUser> Register(RegistrationRequestDto registrationRequestDto)
        {
            LocalUser localUser = _mapper.Map<LocalUser>(registrationRequestDto);
            _dbContext.LocalUsers.Add(localUser);
            await _dbContext.SaveChangesAsync();
            localUser.Password = "";
            return localUser;
        }
    }
}
