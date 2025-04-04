using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DotBot.Models.DTOs.User
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required"), EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
    }
}
