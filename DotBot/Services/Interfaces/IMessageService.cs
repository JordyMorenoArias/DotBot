using DotBot.Models.Entities;

namespace DotBot.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for services handling message operations and management.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Adds a new message to the specified user's conversation.
        /// </summary>
        /// <param name="userId">The unique identifier of the user sending the message.</param>
        /// <param name="message">The message entity to be added.</param>
        /// <returns>The added message with server-generated properties.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the message is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the user is not found or invalid.</exception>
        Task<Message> AddMessage(int userId, Message message);

        /// <summary>
        /// Deletes a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message to delete.</param>
        /// <returns>
        /// True if the message was successfully deleted; 
        /// False if the message was not found.
        /// </returns>
        Task<bool> DeleteMessage(int id);

        /// <summary>
        /// Retrieves a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message.</param>
        /// <returns>
        /// The message if found; otherwise, null.
        /// </returns>
        Task<Message?> GetMessageById(int id);

        /// <summary>
        /// Retrieves all messages belonging to a specific chat session.
        /// </summary>
        /// <param name="chatSessionId">The unique identifier of the chat session.</param>
        /// <returns>
        /// A collection of messages for the specified chat session.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the chat session is not found.</exception>
        Task<IEnumerable<Message>> GetMessagesByChatSessionId(int chatSessionId);

        /// <summary>
        /// Updates the message.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="message">The message.</param>
        /// <returns>The updated message entity with all changes persisted.</returns>
        Task<Message> UpdateMessage(int userId, Message message);
    }
}