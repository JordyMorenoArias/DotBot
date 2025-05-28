using DotBot.Models.DTOs.Message;
using DotBot.Models.Entities;

namespace DotBot.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for services handling chat bot operations and interactions.
    /// </summary>
    public interface IChatBotService
    {
        /// <summary>
        /// Generates a custom AI response based on conversation history and a specific prompt.
        /// </summary>
        /// <param name="messages">The conversation history as a collection of messages.</param>
        /// <param name="customPrompt">The specific instructions or context to guide the AI's response.</param>
        /// <returns>A task that represents the asynchronous operation, containing the generated response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when messages or customPrompt is null.</exception>
        /// <exception cref="ArgumentException">Thrown when customPrompt is empty or whitespace.</exception>
        Task<string> GenerateCustomResponse(IEnumerable<Message> messages, string customPrompt);

        /// <summary>
        /// Processes a complete chat interaction including message handling and response generation.
        /// </summary>
        /// <param name="userId">The unique identifier of the user initiating the interaction.</param>
        /// <param name="messageAdd">The data transfer object containing the message details to add.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing the collection
        /// of messages including the new message and any system responses.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when messageAdd is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the specified user is not found.</exception>
        Task<IEnumerable<Message>> ProcessChatInteraction(int userId, MessageAddDto messageAdd);
    }
}