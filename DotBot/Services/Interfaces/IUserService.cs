using DotBot.Models.DTOs.User;

namespace DotBot.Services.Interfaces
{
    public interface IUserService
    {
        UserAuthenticatedDto GetAuthenticatedUser(HttpContext httpContext);
        Task<UserDto> GetUserByEmail(string email);
        Task<UserDto> GetUserById(int id);
    }
}