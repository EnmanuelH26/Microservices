using System.ComponentModel.DataAnnotations;

namespace App.Web.Models
{
    public class LoginResponseDto
    {
        [Required]
        public UserDto User { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
