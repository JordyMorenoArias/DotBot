namespace DotBot.Models.DTOs.ChatSession
{
    public class ChatSessionUpdateDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
