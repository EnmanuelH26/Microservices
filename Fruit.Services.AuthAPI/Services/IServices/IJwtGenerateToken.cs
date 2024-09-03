using Fruit.Services.AuthAPI.Models;

namespace Fruit.Services.AuthAPI.Services.IServices
{
    public interface IJwtGenerateToken
    {
        string GenerateJSONWebToken(AplicationUser aplicationUser);
    }
}
