using App.Web.Models;
using App.Web.Service;
using App.Web.Service.IService;
using App.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public IActionResult Login()
        { 
           LoginRequestDto model = new LoginRequestDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            var response = await _authService.LoginAsync(requestDto);

            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result)); // Convertir el resultado en un objeto LoginResponseDto


                await SignUser(loginResponseDto); // Iniciar sesión
                _tokenProvider.SetToken(loginResponseDto.Token); // Guardar el token en la cookie

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = response?.Message;
                return View(requestDto);    
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem> // el SelectListItem es un objeto que representa un elemento de una lista desplegable
            {
                new SelectListItem() { Value = SD.RoleAdmin, Text = SD.RoleAdmin },
                new SelectListItem() { Value = SD.RoleCustomer, Text = SD.RoleCustomer }
            };

            ViewBag.RoleList = roleList; // ViewBag es una bolsa de datos que se puede pasar a la vista

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto requestDto)
        {
            var result = await _authService.RegisterAsync(requestDto);
            ResponseDto assignRole;

            if(result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(requestDto.Role)){
                    requestDto.Role = SD.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAsync(requestDto);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["Success"] = "Registration success";
                    return RedirectToAction("Login");
                }

            }
            else
            {
                TempData["Error"] = result?.Message;
            }
            var roleList = new List<SelectListItem> // el SelectListItem es un objeto que representa un elemento de una lista desplegable
            {
                new SelectListItem() { Value = SD.RoleAdmin, Text = SD.RoleAdmin },
                new SelectListItem() { Value = SD.RoleCustomer, Text = SD.RoleCustomer }
            };


            ViewBag.RoleList = roleList; // ViewBag es una bolsa de datos que se puede pasar a la vista

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("index", "Home");
        }



        private async Task SignUser(LoginResponseDto login)
        {

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(login.Token);

            var identity = new ClaimsIdentity(jwt.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));



            var principal = new ClaimsPrincipal(identity); // ClaimsPrincipal es un objeto que representa la identidad del usuario

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
