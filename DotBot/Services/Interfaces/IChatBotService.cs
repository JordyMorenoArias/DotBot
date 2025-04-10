using DotBot.Models.DTOs.Message;
using DotBot.Models.Entities;

namespace DotBot.Services.Interfaces
{
    public interface IChatBotService
    {
        Task<IEnumerable<Message>?> HandleUserPrompt(MessageAddDto messageAdd);
    }
}