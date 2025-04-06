using DotBot.Models.DTOs.Message;

namespace DotBot.Services.Interfaces
{
    public interface IChatBotService
    {
        Task<string?> HandleUserPrompt(MessageAddDto messageAdd);
    }
}