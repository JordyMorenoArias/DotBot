using DotBot.Models.Entities;

namespace DotBot.Repositories.Interfaces
{
    public interface IChatSessionRepository
    {
        Task<bool> AddChatSession(ChatSession chatSession);
        Task<bool> DeleteChatSession(int id);
        Task<bool> DeleteChatSessionsByUserId(int userId);
        Task<ChatSession?> GetChatSessionById(int id);
        Task<IEnumerable<ChatSession>> GetChatSessionsByUserId(int userId);
        Task<bool> UpdateChatSession(ChatSession chatSession);
    }
}