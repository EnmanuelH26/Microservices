using Fruit.Services.AuthAPI.Data;
using Fruit.Services.AuthAPI.Models;
using Fruit.Services.AuthAPI.Models.Dto;
using Fruit.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace Fruit.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {

        //inyectamos el contexto de la base de datos, el usermanager, el rolemanager y el servicio de jwt
        private readonly AppDbContext _db;
        private readonly UserManager<AplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtGenerateToken _jwtGenerateToken;
        public AuthService(AppDbContext db,
               UserManager<AplicationUser> user, 
               RoleManager<IdentityRole> role, 
               IJwtGenerateToken jwtGenerateToken,
               RoleManager<IdentityRole> roleManager
               )
        {
            _db = db;
            _user = user;
            _role = role;
            _jwtGenerateToken = jwtGenerateToken;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignRole(string email, string role)
        {
            var user =_db.AplicationUser.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();

                }
                await _user.AddToRoleAsync(user, role);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto userForLogin)
        {
            var user =  _db.AplicationUser.FirstOrDefault(u => u.UserName.ToLower() == userForLogin.UserName.ToLower());
            bool isValid = await _user.CheckPasswordAsync(user, userForLogin.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null , Token = "" }; 
            }
            //is the user was found and the password is correct generate token
            var token = _jwtGenerateToken.GenerateJSONWebToken(user);
            UserDto userDto = new()
            {
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                ID = user.Id

            };

            LoginResponseDto loguinResponse = new()
            {
                User = userDto,
                Token = token
            };

            return loguinResponse;
        }

        public async Task<string> Register(RegistrationRequestDto userForRegistration)
        {
            AplicationUser user = new()
            {
                UserName = userForRegistration.Email,
                Email = userForRegistration.Email,
                NormalizedEmail = userForRegistration.Email.ToUpper(),
                Name = userForRegistration.Name,
                PhoneNumber = userForRegistration.PhoneNumber
            };

            try
            {
                var result = await _user.CreateAsync(user, userForRegistration.Password);
                if (result.Succeeded)
                {
                    var userReturn = _db.AplicationUser.FirstOrDefault(u => u.Email == userForRegistration.Email); 
                    UserDto userDto = new()
                    {
                        Email = userReturn.Email,
                        Name = userReturn.Name,
                        PhoneNumber = userReturn.PhoneNumber,
                        ID = userReturn.Id
                    };
                    return "";

                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }catch(Exception ex)
            {
              
            }
            return "Error";
        }
    }
}
