using DotBot.Constrants;
using System.ComponentModel.DataAnnotations;

namespace DotBot.Models.DTOs.Message
{
    public class MessageAddDto
    {
        public int ChatSessionId { get; set; }

        public int UserId { get; set; }

        public Role Role { get; set; } = Role.user;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
