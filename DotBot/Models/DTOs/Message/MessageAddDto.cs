using DotBot.Constrants;
using System.ComponentModel.DataAnnotations;

namespace DotBot.Models.DTOs.Message
{
    public class MessageAddDto
    {
        public int ChatSessionId { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
