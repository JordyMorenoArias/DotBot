using DotBot.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotBot.Data
{
    public class DotBotContext : DbContext
    {
        public DotBotContext(DbContextOptions<DotBotContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
