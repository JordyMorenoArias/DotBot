using DotBot.Models.Entities;

namespace DotBot.Repositories.Interfaces
{
    public interface IChatSessionRepository
    {
        Task<ChatSession> AddChatSession(ChatSession chatSession);
        Task<bool> DeleteChatSession(int id);
        Task<bool> DeleteChatSessionsByUserId(int userId);
        Task<ChatSession?> GetChatSessionById(int id);
        Task<ChatSession?> GetChatSessionByIdWithMessages(int id);
        Task<IEnumerable<ChatSession>> GetChatSessionsByUserId(int userId);
        Task<ChatSession?> GetMostRecentSessionByUserId(int userId);
        Task<bool> UpdateChatSession(ChatSession chatSession);
    }
}