using DotBot.Data;
using DotBot.Models.Entities;
using DotBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotBot.Repositories
{
    /// <summary>
    /// Represents a repository for managing chat sessions in the database.
    /// </summary>
    /// <seealso cref="DotBot.Repositories.Interfaces.IChatSessionRepository" />
    public class ChatSessionRepository : IChatSessionRepository
    {
        private readonly DotBotContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatSessionRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to be used for data operations.</param>
        public ChatSessionRepository(DotBotContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the chat session by identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the chat session.</param>
        /// <returns>The chat session if found; otherwise, null.</returns>
        public async Task<ChatSession?> GetChatSessionById(int id)
        {
            return await _context.ChatSessions
                .FirstOrDefaultAsync(cs => cs.Id == id);
        }

        /// <summary>
        /// Gets the chat session by identifier with messages.
        /// </summary>
        /// <param name="id">The unique identifier of the chat session.</param>
        /// <returns>The chat session with its messages if found; otherwise, null.</returns>
        public async Task<ChatSession?> GetChatSessionByIdWithMessages(int id)
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
                .OrderByDescending(cs => cs.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the most recent session by user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The most recent chat session for the specified user if found; otherwise, null.</returns>
        public async Task<ChatSession?> GetMostRecentSessionByUserId(int userId)
        {
            return await _context.ChatSessions
                .Where(cs => cs.UserId == userId)
                .OrderByDescending(cs => cs.CreatedAt)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds the chat session.
        /// </summary>
        /// <param name="chatSession">The chat session entity to add.</param>
        /// <returns>The added chat session with updated database-generated fields.</returns>
        public async Task<ChatSession> AddChatSession(ChatSession chatSession)
        {
            await _context.ChatSessions.AddAsync(chatSession);
            await _context.SaveChangesAsync();
            return chatSession;
        }

        /// <summary>
        /// Updates the chat session.
        /// </summary>
        /// <param name="chatSession">The chat session entity to update.</param>
        /// <returns>The updated chat session.</returns>
        public async Task<ChatSession> UpdateChatSession(ChatSession chatSession)
        {
            _context.ChatSessions.Update(chatSession);
            await _context.SaveChangesAsync();
            return chatSession;
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
    }
}