using DotBot.Models.DTOs.ChatSession;
using DotBot.Models.Entities;

namespace DotBot.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for services handling chat session operations.
    /// </summary>
    public interface IChatSessionService
    {
        /// <summary>
        /// Creates a new chat session for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The newly created chat session.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified user is not found.</exception>
        Task<ChatSession> AddChatSession(int userId);

        /// <summary>
        /// Deletes a chat session by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the chat session to delete.</param>
        /// <returns>True if the chat session was successfully deleted; otherwise false.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified chat session is not found.</exception>
        Task<bool> DeleteChatSession(int id);

        /// <summary>
        /// Retrieves a chat session by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the chat session.</param>
        /// <returns>The chat session if found; otherwise null.</returns>
        Task<ChatSession?> GetChatSessionById(int id);

        /// <summary>
        /// Retrieves a chat session by its unique identifier including all associated messages.
        /// </summary>
        /// <param name="id">The unique identifier of the chat session.</param>
        /// <returns>The chat session with messages if found; otherwise null.</returns>
        Task<ChatSession?> GetChatSessionByIdWithMessages(int id);

        /// <summary>
        /// Retrieves all chat sessions belonging to the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of the user's chat sessions.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified user is not found.</exception>
        Task<IEnumerable<ChatSession>> GetChatSessionsByUserId(int userId);

        /// <summary>
        /// Retrieves the most recent chat session for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The most recent chat session if found; otherwise null.</returns>
        Task<ChatSession?> GetMostRecentSessionByUserId(int userId);

        /// <summary>
        /// Updates an existing chat session with new information.
        /// </summary>
        /// <param name="newChatSession">The data transfer object containing updated chat session information.</param>
        /// <returns>The updated chat session.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when either the user or chat session is not found.
        /// </exception>
        Task<ChatSession> UpdateChatSession(ChatSessionUpdateDto newChatSession);
    }
}