using Microsoft.AspNetCore.Mvc;
using System.Net;
using Villafy_Api.Models;
using Villafy_Api.Models.Dto.Auth;
using Villafy_Api.Repository.IRepository;

namespace Villafy_Api.Controllers
{
    [Route("Api/UsersAuth")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            this._response = new();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _userRepository.Login(loginRequestDto);
            if (loginResponse.LocalUser == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or Password is incorrect");
                _response.Result = loginResponse;

                return BadRequest(_response);
            }
            _response.statusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            bool ifUserUnique = _userRepository.IsUserUnique(registrationRequestDto.UserName);
            if (!ifUserUnique)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User Already Exists");
                return BadRequest(_response);
            }

            var user = await _userRepository.Register(registrationRequestDto);
            if (user == null)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error While Registring");
                return BadRequest(_response);
            }
            _response.statusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            //TODO:Test
            _response.Result = registrationRequestDto;
            return Ok(_response);
        }
    }
}
