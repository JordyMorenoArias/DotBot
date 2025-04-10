using DotBot.Models.DTOs.ChatSession;
using DotBot.Models.Entities;
using DotBot.Repositories.Interfaces;
using DotBot.Services.Interfaces;

namespace DotBot.Services
{
    /// <summary>
    /// Service responsible for managing chat sessions associated with users.
    /// </summary>
    public class ChatSessionService : IChatSessionService
    {
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IUserService _userService;

        public ChatSessionService(IChatSessionRepository chatSessionRepository, IUserService userService)
        {
            _chatSessionRepository = chatSessionRepository;
            _userService = userService;
        }

        /// <summary>
        /// Retrieves a chat session by its ID.
        /// </summary>
        /// <param name="id">The ID of the chat session.</param>
        /// <returns>The chat session if found; otherwise, null.</returns>
        public async Task<ChatSession?> GetChatSessionById(int id)
        {
            return await _chatSessionRepository.GetChatSessionById(id);
        }

        /// <summary>
        /// Gets the chat session by identifier with messages.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ChatSession?> GetChatSessionByIdWithMessages(int id)
        {
            return await _chatSessionRepository.GetChatSessionByIdWithMessages(id);
        }

        /// <summary>
        /// Retrieves all chat sessions associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of chat sessions.</returns>
        public async Task<IEnumerable<ChatSession>> GetChatSessionsByUserId(int userId)
        {
            return await _chatSessionRepository.GetChatSessionsByUserId(userId);
        }

        /// <summary>
        /// Retrieves the most recent chat session for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The most recent chat session if it exists; otherwise, null.</returns>
        public async Task<ChatSession?> GetMostRecentSessionByUserId(int userId)
        {
            return await _chatSessionRepository.GetMostRecentSessionByUserId(userId);
        }

        /// <summary>
        /// Creates a new chat session for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The created chat session.</returns>
        /// <exception cref="ArgumentException">Thrown when the user is not found.</exception>
        public async Task<ChatSession?> AddChatSession(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
                throw new ArgumentException("User not found");

            var chatSession = new ChatSession
            {
                UserId = userId
            };

            return await _chatSessionRepository.AddChatSession(chatSession);
        }

        /// <summary>
        /// Updates the title of a chat session.
        /// </summary>
        /// <param name="chatSessionUpdate">DTO containing the updated title and session info.</param>
        /// <returns>True if the update is successful; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown when user or chat session is not found.</exception>
        public async Task<bool> UpdateChatSessionTitle(ChatSessionUpdateDto chatSessionUpdate)
        {
            var user = await _userService.GetUserById(chatSessionUpdate.UserId);

            if (user == null)
                throw new ArgumentException("User not found");

            var chatSession = await _chatSessionRepository.GetChatSessionById(chatSessionUpdate.Id);

            if (chatSession == null)
                throw new ArgumentException("Chat session not found");

            if (chatSessionUpdate.NewTitle != null)
                chatSession.Title = chatSessionUpdate.NewTitle;

            return await _chatSessionRepository.UpdateChatSession(chatSession);
        }

        /// <summary>
        /// Deletes a chat session by its ID.
        /// </summary>
        /// <param name="id">The ID of the chat session to delete.</param>
        /// <returns>True if deletion is successful; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown when the chat session is not found.</exception>
        public async Task<bool> DeleteChatSession(int id)
        {
            var chatSession = await _chatSessionRepository.GetChatSessionById(id);

            if (chatSession == null)
                throw new ArgumentException("Chat session not found");

            return await _chatSessionRepository.DeleteChatSession(id);
        }

        /// <summary>
        /// Deletes all chat sessions associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose chat sessions are to be deleted.</param>
        /// <returns>True if deletion is successful; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown when the user is not found.</exception>
        public async Task<bool> DeleteChatSessionsByUserId(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
                throw new ArgumentException("User not found");

            return await _chatSessionRepository.DeleteChatSessionsByUserId(userId);
        }
    }

}
