using DotBot.Models.Entities;

namespace DotBot.Services.Security.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}