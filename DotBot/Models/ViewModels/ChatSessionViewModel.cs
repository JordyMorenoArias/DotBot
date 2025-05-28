using DotBot.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DotBot.Models.ViewModels
{
    public class ChatSessionViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }


        [MaxLength(50)]
        public string? Title { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
