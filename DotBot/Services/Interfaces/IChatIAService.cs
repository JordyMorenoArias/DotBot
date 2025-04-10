
using DotBot.Models.DTOs.Message;

namespace DotBot.Services.Interfaces
{
    public interface IChatIAService
    {
        Task<string?> GetChatGptResponse(IEnumerable<ChatMessage> messages);
    }
}