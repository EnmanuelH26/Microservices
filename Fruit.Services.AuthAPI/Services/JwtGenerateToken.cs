using Fruit.Services.AuthAPI.Models;
using Fruit.Services.AuthAPI.Services.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fruit.Services.AuthAPI.Services
{
    public class JwtGenerateToken : IJwtGenerateToken
    {
        private readonly Jwtoptions _jwOptions;
        public JwtGenerateToken(IOptions<Jwtoptions>jwOptions)
        {
            _jwOptions = jwOptions.Value; 
        }
        public string GenerateJSONWebToken(AplicationUser aplicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwOptions.Secret); //esto convierte el string en un array de bytes

            var claimList = new List<Claim>
            {
                //creamos una lista de claims, que son los datos que queremos que tenga el token
                new Claim(JwtRegisteredClaimNames.Email, aplicationUser.Name),
                new Claim(JwtRegisteredClaimNames.Sub, aplicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Name, aplicationUser.UserName)
            };

            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); //agregamos los roles a la lista de claims


            //Despues de crear la lista de claims, creamos el token descriptor
            var tokenDesciprtor = new SecurityTokenDescriptor
            {
                Audience = _jwOptions.Audience,//audience es el publico al que va dirigido el token
                Issuer = _jwOptions.Issuer, //issuer es el emisor del token
                Subject = new ClaimsIdentity(claimList), //subject es el sujeto del token
                Expires = DateTime.Now.AddDays(7), //el token expira en 7 dias
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //firmamos el token con el algoritmo HmacSha256Signature
            };
            var token = tokenHandler.CreateToken(tokenDesciprtor); //crea el token con la configuracion que le pasamos

            return tokenHandler.WriteToken(token); //returna el token en formato string
        }
    }
}
