using DotBot.Models.DTOs.User;
using DotBot.Repositories.Interfaces;
using DotBot.Services.Interfaces;

namespace DotBot.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// Retrieves the authenticated user's information from the HTTP context.
        /// </summary>
        /// <param name="httpContext">The current HTTP context containing the user claims.</param>
        /// <returns>A <see cref="UserAuthenticatedDto"/> with the user's ID and email.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when required claims are missing or invalid.</exception>
        public UserAuthenticatedDto GetAuthenticatedUser(HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.FindFirst("Id")?.Value;
            var userEmaimClaim = httpContext.User.FindFirst("Email")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(userEmaimClaim))
                throw new UnauthorizedAccessException("Invalid token or unauthorized access.");

            return new UserAuthenticatedDto
            {
                Id = int.Parse(userIdClaim),
                Email = userEmaimClaim
            };
        }

        /// <summary>
        /// Retrieves user information based on the provided user ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A <see cref="UserDto"/> containing the user's data.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                CreatedAt = user.CreatedAt
            };
        }

        /// <summary>
        /// Retrieves user information based on the provided email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>A <see cref="UserDto"/> containing the user's data.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
