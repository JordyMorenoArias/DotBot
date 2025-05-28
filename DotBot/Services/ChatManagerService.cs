using Azure;
using DotBot.Controllers;
using DotBot.Models.DTOs.ChatSession;
using DotBot.Models.DTOs.Message;
using DotBot.Models.DTOs.User;
using DotBot.Models.Entities;
using DotBot.Models.ViewModels;
using DotBot.Services.Interfaces;
using Microsoft.Identity.Client;

namespace DotBot.Services
{
    /// <summary>
    /// Service responsible for managing chat sessions and user interactions with the chat bot.
    /// </summary>
    /// <seealso cref="DotBot.Services.Interfaces.IChatManagerService" />
    public class ChatManagerService : IChatManagerService
    {
        private readonly IChatSessionService _chatSessionService;
        private readonly IChatBotService _chatBotService;
        private readonly IMarkdownService _markdownService;
        private readonly ILogger<ChatManagerService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatManagerService"/> class.
        /// </summary>
        /// <param name="chatSessionService">The chat session service.</param>
        /// <param name="chatBotService">The chat bot service.</param>
        /// <param name="markdownService">The markdown service.</param>
        public ChatManagerService(IChatSessionService chatSessionService, IChatBotService chatBotService, IMarkdownService markdownService, ILogger<ChatManagerService> logger)
        {
            _chatSessionService = chatSessionService;
            _chatBotService = chatBotService;
            _markdownService = markdownService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the chat view model asynchronous.
        /// </summary>
        /// <param name="chatSessionId">The chat session identifier.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Chat session not found or does not belong to the user.</exception>
        public async Task<ChatViewModel> GetChatViewModelAsync(int chatSessionId, UserAuthenticatedDto user)
        {
            var chatSessions = await _chatSessionService.GetChatSessionsByUserId(user.Id);

            if (chatSessionId <= 0 || string.IsNullOrEmpty(chatSessionId.ToString()))
            {
                return new ChatViewModel { ChatSessions = chatSessions };
            }

            var currentChatSession = await _chatSessionService.GetChatSessionById(chatSessionId);

            if (currentChatSession == null || currentChatSession.UserId != user.Id)
            {
                throw new KeyNotFoundException("Chat session not found or does not belong to the user.");
            }
            if (currentChatSession.Messages is not null)
            {
                currentChatSession.Messages = _markdownService.ConvertMarkdownToHtml(currentChatSession.Messages).ToList();
            }
            return new ChatViewModel
            {
                ChatSessions = chatSessions,
                CurrentChatSession = currentChatSession
            };
        }

        /// <summary>
        /// Handles the user message asynchronous.
        /// </summary>
        /// <param name="messageAdd">The message add.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Message content cannot be null or empty. - messageAdd</exception>
        /// <exception cref="System.InvalidOperationException">No messages returned from the chat bot service.</exception>
        public async Task<List<Message>> HandleUserMessageAsync(MessageAddDto messageAdd, UserAuthenticatedDto user)
        {
            if (messageAdd == null || string.IsNullOrWhiteSpace(messageAdd.Content))
                throw new ArgumentException("Message content cannot be null or empty.", nameof(messageAdd));

            if (messageAdd.ChatSessionId <= 0)
            {
                var newSession = await _chatSessionService.AddChatSession(user.Id);
                messageAdd.ChatSessionId = newSession.Id;
            }

            var messages = await _chatBotService.ProcessChatInteraction(user.Id, messageAdd);

            var chatSessionUpdate = new ChatSessionUpdateDto
            {
                Id = messageAdd.ChatSessionId,
                UserId = user.Id
            };

            chatSessionUpdate.Title = "New Chat";
            var chatSession = await _chatSessionService.UpdateChatSession(chatSessionUpdate);

            return _markdownService.ConvertMarkdownToHtml(messages).ToList();
        }

        /// <summary>
        /// Creates the title for chat session.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="chatSessionId">The chat session identifier.</param>
        /// <returns></returns>
        public async Task<ChatSessionViewModel> CreateTitleForChatSession(int userId, int chatSessionId)
        {
            var chatSession = await _chatSessionService.GetChatSessionByIdWithMessages(chatSessionId);

            var prompt = "Return only a concise title (maximum 30 characters) for this chat session, based on the previous conversation and the language used. The title must be related to the earlier messages. You may use abbreviations for technical concepts. Do not include any explanation, punctuation, or formatting. Respond with the title only.";

            var chatSessionUpdate = new ChatSessionUpdateDto
            {
                Id = chatSessionId,
                UserId = userId,
            };

            var response = await _chatBotService.GenerateCustomResponse(chatSession!.Messages, prompt);

            chatSessionUpdate.Title = response;
            var newChatSession = await _chatSessionService.UpdateChatSession(chatSessionUpdate);

            return new ChatSessionViewModel
            {
                Id = newChatSession.Id,
                Title = newChatSession.Title,
                UserId = newChatSession.UserId,
                CreatedAt = newChatSession.CreatedAt,
            };
        }

        /// <summary>
        /// Deletes the chat session asynchronous.
        /// </summary>
        /// <param name="chatSessionId">The chat session identifier.</param>
        /// <param name="user">The user.</param>
        /// <exception cref="System.Exception">Chat session not found or does not belong to the user.</exception>
        public async Task DeleteChatSessionAsync(int chatSessionId, UserAuthenticatedDto user)
        {
            var chatSession = await _chatSessionService.GetChatSessionById(chatSessionId);

            if (chatSession == null || chatSession.UserId != user.Id)
                throw new Exception("Chat session not found or does not belong to the user.");

            await _chatSessionService.DeleteChatSession(chatSessionId);
        }
    }
}