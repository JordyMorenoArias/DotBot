using DotBot.Models.Entities;
using DotBot.Services.Security.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotBot.Services.Security
{
    /// <summary>
    /// Service for generating JWT tokens for authenticated users.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtService(IConfiguration configuration)
        {
            _jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>()!;
        }

        /// <summary>
        /// Generates a JWT token for a given user.
        /// </summary>
        /// <param name="user">The user for whom the token is being generated.</param>
        /// <returns>A JWT token string containing user claims.</returns>
        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, _jwtOptions!.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim("Id", user.Id.ToString()),
            new Claim("Email", user.Email),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
