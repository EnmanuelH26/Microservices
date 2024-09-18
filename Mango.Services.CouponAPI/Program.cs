using AutoMapper;
using Mango.Services.CouponAPI.AutoMapper;
using Mango.Services.CouponAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text;

//el builder es el encargado de configurar la aplicacion
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Aqui se configura la conexion a la db
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});
//configuracion del automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();                                         
builder.Services.AddSwaggerGen();
//aqui se configura el jwt para la autenticacion

// Obtener el valor de "Secret" desde la configuración (appsettings.json)
var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret"); // Es una clave secreta utilizada para firmar y verificar la integridad de los tokens JWT.

// Obtener el valor de "Issuer" desde la configuración (appsettings.json)
var Issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer"); // Es una cadena que identifica al emisor del token JWT.

// Obtener el valor de "Audience" desde la configuración (appsettings.json)
var Audience = builder.Configuration.GetValue<string>("ApiSettings:Audience"); // Es una cadena que identifica a la audiencia del token JWT, es decir, quién está destinado a recibir y utilizar el token.


var key = Encoding.ASCII.GetBytes(secret); //esto sirve para convertir la clave secreta en un arreglo de bytes

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
        RequireExpirationTime = true
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
ApplyMigration();
app.Run();

//metodo para agregar cualquier migracion pendiente.
void ApplyMigration()
{
    //se crea un scope para obtener el contexto de la db, un scope es un objeto que se encarga de liberar los recursos
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}