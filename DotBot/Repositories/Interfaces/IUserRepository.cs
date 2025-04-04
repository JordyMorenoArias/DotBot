using DotBot.Models.Entities;

namespace DotBot.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddUser(User user);
        Task<bool> DeleteUser(int id);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int id);
        Task<bool> UpdateUser(User user);
    }
}