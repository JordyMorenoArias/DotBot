using DotBot.Models.DTOs.ChatSession;
using DotBot.Models.Entities;

namespace DotBot.Services.Interfaces
{
    public interface IChatSessionService
    {
        Task<ChatSession?> AddChatSession(int userId);
        Task<bool> DeleteChatSession(int id);
        Task<bool> DeleteChatSessionsByUserId(int userId);
        Task<ChatSession?> GetChatSessionById(int id);
        Task<IEnumerable<ChatSession>> GetChatSessionsByUserId(int userId);
        Task<ChatSession?> GetMostRecentSessionByUserId(int userId);
        Task<bool> UpdateChatSessionTitle(ChatSessionUpdateDto chatSessionUpdate);
    }
}