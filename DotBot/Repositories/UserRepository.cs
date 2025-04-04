using DotBot.Data;
using DotBot.Models.Entities;
using DotBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotBot.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DotBotContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for e-commerce operations.</param>
        public UserRepository(DotBotContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>The added user.</returns>
        public async Task<bool> AddUser(User user)
        {
            var existingUser = await GetUserByEmail(user.Email);
            if (existingUser != null)
                return false;

            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user entity with updated information.</param>
        /// <returns>The updated user if successful; otherwise, null.</returns>
        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await GetUserById(user.Id);
            if (existingUser is null)
                return false;

            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>The deleted user if found; otherwise, null.</returns>
        public async Task<bool> DeleteUser(int id)
        {
            var user = await GetUserById(id);
            if (user is null)
                return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
