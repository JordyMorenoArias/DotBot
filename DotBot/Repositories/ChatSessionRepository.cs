using DotBot.Data;
using DotBot.Models.Entities;
using DotBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotBot.Repositories
{
    public class ChatSessionRepository : IChatSessionRepository
    {
        private readonly DotBotContext _context;

        public ChatSessionRepository(DotBotContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a chat session by its unique identifier, including its related messages.
        /// </summary>
        /// <param name="id">The ID of the chat session to retrieve.</param>
        /// <returns>The chat session with the specified ID, or <c>null</c> if not found.</returns>
        public async Task<ChatSession?> GetChatSessionById(int id)
        {
            return await _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefaultAsync(cs => cs.Id == id);
        }

        /// <summary>
        /// Retrieves all chat sessions associated with a specific user, including their messages.
        /// </summary>
        /// <param name="userId">The ID of the user whose chat sessions are to be retrieved.</param>
        /// <returns>A collection of chat sessions belonging to the specified user.</returns>
        public async Task<IEnumerable<ChatSession>> GetChatSessionsByUserId(int userId)
        {
            return await _context.ChatSessions
                .Include(cs => cs.Messages)
                .Where(cs => cs.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new chat session to the database.
        /// </summary>
        /// <param name="chatSession">The chat session entity to add.</param>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> AddChatSession(ChatSession chatSession)
        {
            await _context.ChatSessions.AddAsync(chatSession);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Updates an existing chat session in the database.
        /// </summary>
        /// <param name="chatSession">The chat session entity with updated values.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> UpdateChatSession(ChatSession chatSession)
        {
            var existingChatSession = await GetChatSessionById(chatSession.Id);

            if (existingChatSession == null)
                return false;

            _context.Entry(existingChatSession).CurrentValues.SetValues(chatSession);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes a chat session from the database based on its ID.
        /// </summary>
        /// <param name="id">The ID of the chat session to delete.</param>
        /// <returns><c>true</c> if the chat session was successfully deleted; otherwise, <c>false</c>.</returns>
        public async Task<bool> DeleteChatSession(int id)
        {
            var chatSession = await GetChatSessionById(id);

            if (chatSession == null)
                return false;

            _context.ChatSessions.Remove(chatSession);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes all chat sessions associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose chat sessions are to be deleted.</param>
        /// <returns><c>true</c> if any chat sessions were deleted; otherwise, <c>false</c>.</returns>
        public async Task<bool> DeleteChatSessionsByUserId(int userId)
        {
            var chatSessions = await GetChatSessionsByUserId(userId);

            if (!chatSessions.Any())
                return false;

            _context.ChatSessions.RemoveRange(chatSessions);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
