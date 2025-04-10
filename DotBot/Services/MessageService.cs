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
        /// Adds a new message to the appropriate chat session.
        /// If no valid session is provided, the most recent session for the user is used.
        /// </summary>
        /// <param name="messageAdd">The DTO containing message details.</param>
        /// <returns>The added message if successful; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentException">Thrown when a valid chat session for the user cannot be found.</exception>
        public async Task<Message?> AddMessage(MessageAddDto messageAdd)
        {
            if (messageAdd.ChatSessionId > 0)
            {
                var chatSession = await _chatSessionService.GetChatSessionById(messageAdd.ChatSessionId);

                if (chatSession == null)
                {
                    var mostRecentSession = await _chatSessionService.GetMostRecentSessionByUserId(messageAdd.UserId)
                        ?? throw new ArgumentException("No valid chat session found for user");

                    messageAdd.ChatSessionId = mostRecentSession.Id;
                }
            }
            else
            {
                var mostRecentSession = await _chatSessionService.GetMostRecentSessionByUserId(messageAdd.UserId)
                    ?? throw new ArgumentException("No valid chat session found for user");

                messageAdd.ChatSessionId = mostRecentSession.Id;
            }

            var message = new Message
            {
                ChatSessionId = messageAdd.ChatSessionId,
                Role = messageAdd.Role,
                Content = messageAdd.Content,
            };

            return await _messageRepository.AddMessage(message);
        }

        /// <summary>
        /// Updates an existing message.
        /// </summary>
        /// <param name="message">The message with updated content.</param>
        /// <returns><c>true</c> if the update is successful; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown when the message does not exist.</exception>
        public async Task<bool> UpdateMessage(Message message)
        {
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
