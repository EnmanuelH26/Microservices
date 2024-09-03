using Fruit.Services.AuthAPI.Data;
using Fruit.Services.AuthAPI.Models;
using Fruit.Services.AuthAPI.Services;
using Fruit.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//agregamos la cadena de conexion
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});
//agregamos la configuracion de jwt en el archivo appsettings.json y la inyectamos en el servicio de jwt
builder.Services.Configure<Jwtoptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));    
// Add services to the container.

builder.Services.AddControllers();
//agregamos los servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtGenerateToken, JwtGenerateToken>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//agregamos la autenticacion y autorizacion, y configuramos el token jwt 
builder.Services.AddIdentity<AplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();
//metodo para agregar cualquier migracion pendiente.
void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}