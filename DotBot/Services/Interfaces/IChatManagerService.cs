using DotBot.Models.DTOs.Message;
using DotBot.Models.DTOs.User;
using DotBot.Models.Entities;
using DotBot.Models.ViewModels;

namespace DotBot.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for services managing chat operations including session management and message handling.
    /// </summary>
    public interface IChatManagerService
    {
        /// <summary>
        /// Generates and assigns a title for the specified chat session based on its content.
        /// </summary>
        /// <param name="userId">The unique identifier of the user owning the chat session.</param>
        /// <param name="chatSessionId">The unique identifier of the chat session to title.</param>
        /// <returns>A <see cref="ChatSessionViewModel"/> containing the updated chat session details including the new title.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description>The specified user is not found</description></item>
        ///   <item><description>The chat session is not found</description></item>
        ///   <item><description>The chat session doesn't belong to the user</description></item>
        /// </list>
        /// </exception>
        Task<ChatSessionViewModel> CreateTitleForChatSession(int userId, int chatSessionId);

        /// <summary>
        /// Permanently deletes a chat session and all associated messages after validating user permissions.
        /// </summary>
        /// <param name="chatSessionId">The unique identifier of the chat session to delete.</param>
        /// <param name="user">The authenticated user DTO containing user identity information.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description>The chat session is not found</description></item>
        ///   <item><description>The user doesn't have permission to delete the session</description></item>
        /// </list>
        /// </exception>
        Task DeleteChatSessionAsync(int chatSessionId, UserAuthenticatedDto user);

        /// <summary>
        /// Retrieves the complete chat view model including session details and message history.
        /// </summary>
        /// <param name="chatSessionId">The unique identifier of the chat session.</param>
        /// <param name="user">The authenticated user DTO containing user identity information.</param>
        /// <returns>A <see cref="ChatViewModel"/> containing all chat session data and messages.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description>The chat session is not found</description></item>
        ///   <item><description>The user doesn't have access to the session</description></item>
        /// </list>
        /// </exception>
        Task<ChatViewModel> GetChatViewModelAsync(int chatSessionId, UserAuthenticatedDto user);

        /// <summary>
        /// Processes a new user message, generates AI responses, and manages the conversation flow.
        /// </summary>
        /// <param name="messageAdd">The DTO containing the message content and metadata.</param>
        /// <param name="user">The authenticated user DTO containing user identity information.</param>
        /// <returns>A list of <see cref="Message"/> objects representing the user message and any system responses.</returns>
        /// <exception cref="ArgumentNullException">Thrown when messageAdd or user is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description>The chat session is not found</description></item>
        ///   <item><description>The user doesn't have access to the session</description></item>
        ///   <item><description>Message content validation fails</description></item>
        /// </list>
        /// </exception>
        Task<List<Message>> HandleUserMessageAsync(MessageAddDto messageAdd, UserAuthenticatedDto user);
    }
}