using DotBot.Models.Entities;

namespace DotBot.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<bool> AddMessage(Message message);
        Task<bool> DeleteMessage(int id);
        Task<Message?> GetMessageById(int id);
        Task<bool> UpdateMessage(Message message);
    }
}