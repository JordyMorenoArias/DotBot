
using DotBot.Models.DTOs.Message;
using DotBot.Models.Entities;

namespace DotBot.Services.Interfaces
{
    public interface IMessageService
    {
        Task<bool> AddMessage(MessageAddDto messageAdd);
        Task<bool> DeleteMessage(int id);
        Task<Message?> GetMessageById(int id);
        Task<bool> UpdateMessage(Message message);
    }
}