using DotBot.Models.DTOs.Auth;
using DotBot.Models.DTOs.User;

namespace DotBot.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(UserLoginDto loginDto);
        Task<bool> Register(UserRegisterDto userRegister);
    }
}