namespace Villafy_Api.Models.Dto.Auth
{
    public class LoginResponseDto
    {
        public LocalUser LocalUser { get; set; }
        public string Token { get; set; }
    }
}
