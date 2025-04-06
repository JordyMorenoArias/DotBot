using DotBot.Models.Entities;

namespace DotBot.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message?> AddMessage(Message message);
        Task<bool> DeleteMessage(int id);
        Task<Message?> GetMessageById(int id);
        Task<IEnumerable<Message>> GetMessagesByChatSessionId(int chatSessionId);
        Task<bool> UpdateMessage(Message message);
    }
}