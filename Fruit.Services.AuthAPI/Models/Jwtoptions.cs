namespace Fruit.Services.AuthAPI.Models
{
    public class Jwtoptions
    {
        public string? Secret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
    }
}
