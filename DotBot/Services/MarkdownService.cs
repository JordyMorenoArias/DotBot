using DotBot.Models.Entities;
using DotBot.Models.ViewModels;
using DotBot.Services.Interfaces;
using Markdig;

namespace DotBot.Services
{
    public class MarkdownService : IMarkdownService
    {
        /// <summary>
        /// Converts the Markdown content of each message to HTML.
        /// </summary>
        /// <param name="messages">A collection of messages with Markdown content.</param>
        /// <returns>
        /// A collection of new <see cref="Message"/> objects with their <c>Content</c> converted to HTML.
        /// </returns>
        public IEnumerable<Message> ConvertMarkdownToHtml(IEnumerable<Message> messages)
        {
            return messages.Select(message => new Message
            {
                Id = message.Id,
                ChatSessionId = message.ChatSessionId,
                Role = message.Role,
                Content = Markdown.ToHtml(message.Content),
                CreatedAt = message.CreatedAt
            });
        }
    }
}
