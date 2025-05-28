using DotBot.Models.Entities;

namespace DotBot.Repositories.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository managing chat session operations.
    /// </summary>
    public interface IChatSessionRepository
    {
        /// <summary>
        /// Adds a new chat session to the repository.
        /// </summary>
        /// <param name="chatSession">The chat session entity to add.</param>
        /// <returns>The added chat session with updated database-generated fields.</returns>
        Task<ChatSession> AddChatSession(ChatSession chatSession);

        /// <summary>
        /// Deletes a chat session from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the chat session to delete.</param>
        /// <returns>True if the chat session was successfully deleted; otherwise false.</returns>
        Task<bool> DeleteChatSession(int id);

        /// <summary>
        /// Retrieves a chat session by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the chat session.</param>
        /// <returns>The chat session if found; otherwise null.</returns>
        Task<ChatSession?> GetChatSessionById(int id);

        /// <summary>
        /// Retrieves a chat session by its unique identifier including associated messages.
        /// </summary>
        /// <param name="id">The unique identifier of the chat session.</param>
        /// <returns>The chat session with its messages if found; otherwise null.</returns>
        Task<ChatSession?> GetChatSessionByIdWithMessages(int id);

        /// <summary>
        /// Retrieves all chat sessions for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of chat sessions belonging to the specified user.</returns>
        Task<IEnumerable<ChatSession>> GetChatSessionsByUserId(int userId);

        /// <summary>
        /// Retrieves the most recent chat session for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The most recent chat session for the specified user if found; otherwise null.</returns>
        Task<ChatSession?> GetMostRecentSessionByUserId(int userId);

        /// <summary>
        /// Updates an existing chat session in the repository.
        /// </summary>
        /// <param name="chatSession">The chat session entity to update.</param>
        /// <returns>The updated chat session.</returns>
        Task<ChatSession> UpdateChatSession(ChatSession chatSession);
    }
}