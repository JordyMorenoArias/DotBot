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

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatSessionService"/> class.
        /// </summary>
        /// <param name="chatSessionRepository">The chat session repository for data operations.</param>
        /// <param name="userService">The user service for user-related operations.</param>
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
        /// Gets the chat session by its identifier including all associated messages.
        /// </summary>
        /// <param name="id">The unique identifier of the chat session.</param>
        /// <returns>The chat session with messages if found; otherwise, null.</returns>
        public async Task<ChatSession?> GetChatSessionByIdWithMessages(int id)
        {
            return await _chatSessionRepository.GetChatSessionByIdWithMessages(id);
        }

        /// <summary>
        /// Retrieves all chat sessions associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of chat sessions.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified user is not found.</exception>
        public async Task<IEnumerable<ChatSession>> GetChatSessionsByUserId(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
                throw new ArgumentException("User not found");

            return await _chatSessionRepository.GetChatSessionsByUserId(userId);
        }

        /// <summary>
        /// Gets the most recent chat session for a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The most recent chat session if found; otherwise, null.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified user is not found.</exception>
        public async Task<ChatSession?> GetMostRecentSessionByUserId(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
                throw new ArgumentException("User not found");

            return await _chatSessionRepository.GetMostRecentSessionByUserId(userId);
        }

        /// <summary>
        /// Creates a new chat session for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The created chat session.</returns>
        /// <exception cref="ArgumentException">Thrown when the user is not found.</exception>
        public async Task<ChatSession> AddChatSession(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
                throw new ArgumentException("User not found");

            var chatSession = new ChatSession
            {
                UserId = userId
            };

            chatSession = await _chatSessionRepository.AddChatSession(chatSession);
            return chatSession;
        }

        /// <summary>
        /// Updates an existing chat session with new information.
        /// </summary>
        /// <param name="newChatSession">The DTO containing updated chat session data.</param>
        /// <returns>The updated chat session.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when either the user or chat session is not found.
        /// </exception>
        public async Task<ChatSession> UpdateChatSession(ChatSessionUpdateDto newChatSession)
        {
            var user = await _userService.GetUserById(newChatSession.UserId);

            if (user == null)
                throw new ArgumentException("User not found");

            var oldChatSession = await _chatSessionRepository.GetChatSessionById(newChatSession.Id);

            if (oldChatSession == null)
                throw new ArgumentException("Chat session not found");

            oldChatSession.Title = newChatSession.Title;

            return await _chatSessionRepository.UpdateChatSession(oldChatSession);
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
    }
}