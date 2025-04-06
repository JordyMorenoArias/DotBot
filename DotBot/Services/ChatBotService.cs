using DotBot.Constrants;
using DotBot.Models.DTOs.Message;
using DotBot.Models.Entities;
using DotBot.Services.Interfaces;

namespace DotBot.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly IChatIAService _chatIaService;
        private readonly IMessageService _messageService;

        public ChatBotService(IChatIAService chatGptService, IMessageService messageService)
        {
            _chatIaService = chatGptService;
            _messageService = messageService;
        }

        /// <summary>
        /// Handles a user's message prompt by saving it, retrieving the conversation history,
        /// sending it to the AI service, saving the AI's response, and returning the response.
        /// </summary>
        /// <param name="messageAdd">The user's message to be processed.</param>
        /// <returns>The AI-generated response, or null if processing fails.</returns>
        public async Task<string?> HandleUserPrompt(MessageAddDto messageAdd)
        {
            var message = await _messageService.AddMessage(messageAdd);

            if (message == null)
                return null;

            var messages = await _messageService.GetMessagesByChatSessionId(message.ChatSessionId);

            var prompt = messages.Select(m => new ChatMessage
            {
                Role = m.Role.ToString(),
                Content = m.Content
            }).ToList();

            var response = await _chatIaService.GetChatGptResponse(prompt);

            if (response == null)
                return null;

            var assistantMessage = new MessageAddDto
            {
                ChatSessionId = message.ChatSessionId,
                Role = Role.assistant,
                Content = response
            };

            await _messageService.AddMessage(assistantMessage);

            return response;
        }
    }

}
