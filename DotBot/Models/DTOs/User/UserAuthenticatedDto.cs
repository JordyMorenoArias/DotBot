using System.ComponentModel.DataAnnotations;

namespace DotBot.Models.DTOs.User
{
    public class UserAuthenticatedDto
    {
        [Required]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
