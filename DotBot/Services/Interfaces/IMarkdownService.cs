using DotBot.Models.Entities;

namespace DotBot.Services.Interfaces
{
    public interface IMarkdownService
    {
        IEnumerable<Message> ConvertMarkdownToHtml(IEnumerable<Message> messages);
    }
}