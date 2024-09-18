using Microsoft.AspNetCore.Identity;

namespace Fruit.Services.AuthAPI.Models
{
    public class AplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
