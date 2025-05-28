using DotBot.Models.Entities;

namespace DotBot.Repositories.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository handling message persistence operations.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Adds a new message to the repository.
        /// </summary>
        /// <param name="message">The message entity to be added.</param>
        /// <returns>The added message with any database-generated fields populated.</returns>
        Task<Message> AddMessage(Message message);

        /// <summary>
        /// Deletes a message from the repository by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message to delete.</param>
        /// <returns>True if the message was successfully deleted; false if the message wasn't found.</returns>
        Task<bool> DeleteMessage(int id);

        /// <summary>
        /// Retrieves a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message.</param>
        /// <returns>The message if found; otherwise null.</returns>
        Task<Message?> GetMessageById(int id);

        /// <summary>
        /// Retrieves all messages associated with a specific chat session.
        /// </summary>
        /// <param name="chatSessionId">The unique identifier of the chat session.</param>
        /// <returns>A collection of messages belonging to the specified chat session.</returns>
        Task<IEnumerable<Message>> GetMessagesByChatSessionId(int chatSessionId);

        /// <summary>
        /// Updates an existing message in the repository with new content.
        /// </summary>
        /// <param name="message">The message entity containing updated values.</param>
        /// <returns>The updated message entity with persisted changes.</returns>
        Task<Message> UpdateMessage(Message message);
    }
}