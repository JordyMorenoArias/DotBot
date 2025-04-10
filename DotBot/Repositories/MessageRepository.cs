using DotBot.Data;
using DotBot.Models.Entities;
using DotBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotBot.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DotBotContext _context;

        public MessageRepository(DotBotContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a message by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the message to retrieve.</param>
        /// <returns>The message with the specified ID, or <c>null</c> if not found.</returns>
        public async Task<Message?> GetMessageById(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        /// <summary>
        /// Retrieves all messages associated with a specific chat session ID.
        /// </summary>
        /// <param name="chatSessionId">The chat session identifier.</param>
        /// <returns>A collection of messages associated with the specified chat session ID.</returns>
        public async Task<IEnumerable<Message>> GetMessagesByChatSessionId(int chatSessionId)
        {
            return await _context.Messages
                .Where(m => m.ChatSessionId == chatSessionId)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new message to the database.
        /// </summary>
        /// <param name="message">The message entity to add.</param>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public async Task<Message?> AddMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        /// <summary>
        /// Updates an existing message in the database.
        /// </summary>
        /// <param name="message">The message entity with updated values.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> UpdateMessage(Message message)
        {
            var existingMessage = await GetMessageById(message.Id);

            if (existingMessage == null)
                return false;

            _context.Entry(existingMessage).CurrentValues.SetValues(message);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes a message from the database based on its ID.
        /// </summary>
        /// <param name="id">The ID of the message to delete.</param>
        /// <returns><c>true</c> if the message was successfully deleted; otherwise, <c>false</c>.</returns>
        public async Task<bool> DeleteMessage(int id)
        {
            var message = await GetMessageById(id);
            if (message == null)
                return false;
            _context.Messages.Remove(message);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
