using DotBot.Constrants;
using DotBot.Models.DTOs.Message;
using DotBot.Models.Entities;
using DotBot.Services.Interfaces;

namespace DotBot.Services
{
    /// <summary>
    /// Service for managing chat interactions with the AI, including processing user messages and generating responses.
    /// </summary>
    /// <seealso cref="DotBot.Services.Interfaces.IChatBotService" />
    public class ChatBotService : IChatBotService
    {
        private readonly IChatIAService _chatIAService;
        private readonly IMessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatBotService"/> class.
        /// </summary>
        /// <param name="chatGptService">The chat GPT service.</param>
        /// <param name="messageService">The message service.</param>
        public ChatBotService(IChatIAService chatGptService, IMessageService messageService)
        {
            _chatIAService = chatGptService;
            _messageService = messageService;
        }

        /// <summary>
        /// Processes the chat interaction asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="messageAdd">The message add.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">No response received from the AI service.</exception>
        public async Task<IEnumerable<Message>> ProcessChatInteraction(int userId, MessageAddDto messageAdd)
        {
            var message = new Message
            {
                ChatSessionId = messageAdd.ChatSessionId,
                Role = Role.user,
                Content = messageAdd.Content
            };

            var userMessage = await _messageService.AddMessage(userId, message);
            var messages = await _messageService.GetMessagesByChatSessionId(userMessage.ChatSessionId);

            var prompt = messages.Select(m => new ChatMessage
            {
                Role = m.Role.ToString(),
                Content = m.Content
            }).ToList();

            var response = await _chatIAService.GetIAResponse(prompt);

            if (string.IsNullOrEmpty(response))
            {
                await _messageService.DeleteMessage(userMessage.Id);
                throw new InvalidOperationException("No response received from the AI service.");
            }        

            var assistantMessage = new Message
            {
                ChatSessionId = userMessage.ChatSessionId,
                Role = Role.assistant,
                Content = response
            };

            var botMessage = await _messageService.AddMessage(userId, assistantMessage);

            var result = new List<Message>() { userMessage, botMessage};    
            return result;
        }

        /// <summary>
        /// Generates the custom response asynchronous.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <param name="customPrompt">The custom prompt.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Custom prompt cannot be null or empty. - customPrompt</exception>
        public async Task<string> GenerateCustomResponse(IEnumerable<Message> messages, string customPrompt)
        {
            if (string.IsNullOrEmpty(customPrompt))
                throw new ArgumentException("Custom prompt cannot be null or empty.", nameof(customPrompt));

            var fullMessages = messages.Select(m => new ChatMessage
            {
                Role = m.Role.ToString(),
                Content = m.Content
            }).ToList();

            fullMessages.Add(new ChatMessage
            {
                Role = Role.user.ToString(),
                Content = customPrompt
            });

            var response = await _chatIAService.GetIAResponse(fullMessages);

            if (string.IsNullOrEmpty(response))
                throw new InvalidOperationException("No response received from the AI service.");

            return response;
        }
    }
}