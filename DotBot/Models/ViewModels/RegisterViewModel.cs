using System.ComponentModel.DataAnnotations;

namespace DotBot.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required, StringLength(100, ErrorMessage = "Username must be at least {2} characters long.", MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
