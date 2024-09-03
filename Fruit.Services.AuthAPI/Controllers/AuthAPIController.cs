using Fruit.Services.AuthAPI.Models.Dto;
using Fruit.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Fruit.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessages = await _authService.Register(registrationRequestDto);
            if (!string.IsNullOrEmpty(errorMessages))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessages;
                return BadRequest(_response);
            }

            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loguinResponse = await _authService.Login(model);

            if(loguinResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User not found or password is incorrect";
                return BadRequest(_response);
            }

            _response.Result = loguinResponse;
            return Ok(_response);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccesful = await _authService.AssignRole(model.Email, model.Role.ToUpper());

            if (!assignRoleSuccesful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error contountered";
                return BadRequest(_response);
            }

            return Ok(_response);
        }
    }
}
