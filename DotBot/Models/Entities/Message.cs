using DotBot.Constrants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotBot.Models.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int ChatSessionId { get; set; }
        public ChatSession? ChatSession { get; set; }

        public Role Role { get; set; } = Role.user;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
