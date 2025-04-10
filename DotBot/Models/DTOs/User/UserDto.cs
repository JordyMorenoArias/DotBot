using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotBot.Models.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required"), MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required"), EmailAddress, MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
