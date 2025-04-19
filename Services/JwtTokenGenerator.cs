using Microsoft.IdentityModel.Tokens;
using SmartHotelBookingSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace SmartHotelBookingSystem.Services
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserAccount user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()), // Unique User ID
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // Assigns Role-based Claims
            };

            var token = new JwtSecurityToken(
                issuer : _configuration["Jwt:Issuer"],
                audience : _configuration["Jwt:Audience"],
                claims : claims,
                expires: DateTime.Now.AddHours(2), // Token valid for 2 hours
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
