using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DotBot.Models.DTOs.User
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is required"), EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "password is required"), PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
    }
}
