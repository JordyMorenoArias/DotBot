using DotBot.Models.Entities;

namespace DotBot.Models.ViewModels
{
    public class ChatViewModel
    {
        public IEnumerable<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
        public ChatSession? CurrentChatSession { get; set; }
    }
}
