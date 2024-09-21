using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace Mango.Services.CouponAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            // aqui se configura el jwt para la autenticacion

            var settingsSection = builder.Configuration.GetSection("ApiSettings"); //se obtiene la seccion de la configuracion que se llama ApiSettings

            // Obtener el valor de "Secret" desde la configuración (appsettings.json)
            var secret = settingsSection.GetValue<string>("Secret"); // Es una clave secreta utilizada para firmar y verificar la integridad de los tokens JWT.

            // Obtener el valor de "Issuer" desde la configuración (appsettings.json)
            var Issuer = settingsSection.GetValue<string>("Issuer"); // Es una cadena que identifica al emisor del token JWT.

            // Obtener el valor de "Audience" desde la configuración (appsettings.json)
            var Audience = settingsSection.GetValue<string>("Audience"); // Es una cadena que identifica a la audiencia del token JWT, es decir, quién está destinado a recibir y utilizar el token.


            var key = Encoding.ASCII.GetBytes(secret); //esto sirve para convertir la clave secreta en un arreglo de bytes

            //se configura el servicio de autenticacion y se agrega el esquema de autenticacion que sirve para validar el token
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //esquema de autenticacion
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //esquema de desafio de autenticacion (si no se puede autenticar) 
            }).AddJwtBearer(x =>
            {
                //se configuran los parametros de validacion del token
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidateAudience = true,
                    ValidAudience = Audience,
                    ValidateLifetime = true // Valida que el token no haya expirado
                };
            });
            return builder;
        }

    }
}
