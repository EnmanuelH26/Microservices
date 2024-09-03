using Fruit.Services.AuthAPI.Models.Dto;

namespace Fruit.Services.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto userForRegistration);
        Task<LoginResponseDto> Login(LoginRequestDto userForLogin);
        Task<bool> AssignRole(string email, string role);
    } 
}
