
using DotBot.Models.DTOs.Message;

namespace DotBot.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for services that interact with AI chat systems.
    /// </summary>
    public interface IChatIAService
    {
        /// <summary>
        /// Gets an AI-generated response based on the conversation history.
        /// </summary>
        /// <param name="messages">The collection of chat messages representing the conversation history.</param>
        /// <returns>
        /// An AI-generated response message if successful; 
        /// null if no response could be generated or if the input is invalid.
        /// </returns>
        Task<string?> GetIAResponse(IEnumerable<ChatMessage> messages);
    }
}