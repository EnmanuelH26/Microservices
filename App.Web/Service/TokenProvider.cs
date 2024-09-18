using App.Web.Service.IService;
using App.Web.Utility;
using Newtonsoft.Json.Linq;

namespace App.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);    //el httpscontext es el que tiene la informacion de la peticion y esta linea sirve para borrar la cookie
        }

        public string? GetToken()
        {
            string token = null;
            bool hasToken = _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.TokenCookie, out token);
            return hasToken is true ? token : null;

            // todo esto es para obtener el token de la cookie
        }

        public void SetToken(string token)
        {
            //sirve para guardar el token en la cookie
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);                                                               
        }   
    }
}
