using Common.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BonFireAPI.Services
{
    public class TokenService
    {
        public string GenerateToken(User user) 
        {
            Claim[] claims = new Claim[]
            {
                new Claim("loggedUserId", user.Id.ToString()),
                new Claim("role", user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SuperSecretPassword1234SuperSecretPassword1234"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: "bonfire",
                audience: "frontend",
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddDays(1)
            );

            string token =  new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}
