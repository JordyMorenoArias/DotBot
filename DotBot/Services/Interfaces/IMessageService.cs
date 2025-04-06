
using DotBot.Models.DTOs.Message;
using DotBot.Models.Entities;

namespace DotBot.Services.Interfaces
{
    public interface IMessageService
    {
        Task<Message?> AddMessage(MessageAddDto messageAdd);
        Task<bool> DeleteMessage(int id);
        Task<Message?> GetMessageById(int id);
        Task<IEnumerable<Message>> GetMessagesByChatSessionId(int chatSessionId);
        Task<bool> UpdateMessage(Message message);
    }
}