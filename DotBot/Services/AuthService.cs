using DotBot.Models.DTOs.Auth;
using DotBot.Models.DTOs.User;
using DotBot.Models.Entities;
using DotBot.Repositories.Interfaces;
using DotBot.Services.Interfaces;
using DotBot.Services.Security.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DotBot.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IUserRepository userRepository, IJwtService jwtService, PasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Logs in a user with the provided email and password.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="password">User's password.</param>
        /// <returns>An authentication response containing a JWT token and user details.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the email or password is incorrect.</exception>
        public async Task<AuthResponseDto> Login(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmail(loginDto.Email);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password) == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = _jwtService.GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Expires = DateTime.UtcNow.AddHours(3),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    CreatedAt = user.CreatedAt
                }
            };
        }

        /// <summary>
        /// Registers a new user and sends an email verification token.
        /// </summary>
        /// <param name="userRegister">User registration details.</param>
        /// <returns>True if registration is successful, otherwise false.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the email is already registered or registration fails.</exception>
        public async Task<bool> Register(UserRegisterDto userRegister)
        {
            var user = await _userRepository.GetUserByEmail(userRegister.Email);

            if (user != null)
                throw new InvalidOperationException("User with this email already exists");

            userRegister.Password = _passwordHasher.HashPassword(new User(), userRegister.Password);

            User newUser = new User
            {
                Username = userRegister.Username,
                Email = userRegister.Email,
                Password = userRegister.Password,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userRepository.AddUser(newUser);

            if (!result)
                throw new InvalidOperationException("Failed to register user");

            return result;
        }
    }
}
