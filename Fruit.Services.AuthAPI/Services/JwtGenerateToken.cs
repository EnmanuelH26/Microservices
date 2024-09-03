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
        public string GenerateJSONWebToken(AplicationUser aplicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwOptions.Secret); //esto convierte el string en un array de bytes

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, aplicationUser.Name),
                new Claim(JwtRegisteredClaimNames.Sub, aplicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Name, aplicationUser.UserName)
            };

            //Despues de crear la lista de claims, creamos el token descriptor
            var tokenDesciprtor = new SecurityTokenDescriptor
            {
                Audience = _jwOptions.Audience,
                Issuer = _jwOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDesciprtor); //crea el token con la configuracion que le pasamos

            return tokenHandler.WriteToken(token); //returna el token en formato string
        }
    }
}
