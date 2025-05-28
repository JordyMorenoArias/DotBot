using DotBot.Constrants;
using DotBot.Models.DTOs.Message;
using DotBot.Models.Entities;
using DotBot.Repositories;
using DotBot.Repositories.Interfaces;
using DotBot.Services.Interfaces;

namespace DotBot.Services
{
    /// <summary>
    /// Service responsible for handling message operations such as creation, retrieval, update, and deletion.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatSessionService _chatSessionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        /// <param name="messageRepository">The message repository.</param>
        /// <param name="chatSessionService">The chat session service.</param>
        public MessageService(IMessageRepository messageRepository, IChatSessionService chatSessionService)
        {
            _messageRepository = messageRepository;
            _chatSessionService = chatSessionService;
        }

        /// <summary>
        /// Retrieves a message by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the message.</param>
        /// <returns>The message if found; otherwise, <c>null</c>.</returns>
        public async Task<Message?> GetMessageById(int id)
        {
            return await _messageRepository.GetMessageById(id);
        }

        /// <summary>
        /// Retrieves all messages associated with a specific chat session.
        /// </summary>
        /// <param name="chatSessionId">The chat session ID.</param>
        /// <returns>A list of messages linked to the given session.</returns>
        public async Task<IEnumerable<Message>> GetMessagesByChatSessionId(int chatSessionId)
        {
            return await _messageRepository.GetMessagesByChatSessionId(chatSessionId);
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="message">The message.</param>
        /// <returns>The newly added message with all server-generated fields populated.</returns>
        /// <exception cref="System.ArgumentException">
        /// Chat session not found - ChatSessionId
        /// or
        /// Chat session does not belong to the user - ChatSessionId
        /// </exception>
        public async Task<Message> AddMessage(int userId, Message message)
        {
            var chatSession = await _chatSessionService.GetChatSessionById(message.ChatSessionId);

            if (chatSession == null)
                throw new ArgumentException("Chat session not found", nameof(message.ChatSessionId));

            if (chatSession.UserId != userId)
                throw new ArgumentException("Chat session does not belong to the user", nameof(message.ChatSessionId));

            var result = await _messageRepository.AddMessage(message);
            return result;
        }

        /// <summary>
        /// Updates the message.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="message">The message.</param>
        /// <returns>The updated message entity with all changes persisted.</returns>
        /// <exception cref="System.ArgumentException">
        /// Chat session not found - ChatSessionId
        /// or
        /// Chat session does not belong to the user - ChatSessionId
        /// or
        /// Message not found
        /// </exception>
        public async Task<Message> UpdateMessage(int userId, Message message)
        {
            var chatSession = await _chatSessionService.GetChatSessionById(message.ChatSessionId);

            if (chatSession == null)
                throw new ArgumentException("Chat session not found", nameof(message.ChatSessionId));

            if (chatSession.UserId != userId)
                throw new ArgumentException("Chat session does not belong to the user", nameof(message.ChatSessionId));

            var result = await _messageRepository.GetMessageById(message.Id);

            if (result == null)
                throw new ArgumentException("Message not found");

            return await _messageRepository.UpdateMessage(message);
        }

        /// <summary>
        /// Deletes a message by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the message to delete.</param>
        /// <returns><c>true</c> if deletion is successful; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown when the message does not exist.</exception>
        public async Task<bool> DeleteMessage(int id)
        {
            var message = await _messageRepository.GetMessageById(id);

            if (message == null)
                throw new ArgumentException("Message not found");

            return await _messageRepository.DeleteMessage(id);
        }
    }
}