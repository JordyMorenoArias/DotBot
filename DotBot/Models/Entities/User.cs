using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotBot.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required"), MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required"), EmailAddress, MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required"), PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
