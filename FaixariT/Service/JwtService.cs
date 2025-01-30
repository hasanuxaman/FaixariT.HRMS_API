using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FaixariT.HRMS.Service
{
    public class JwtService
    {
        private readonly string _key;
        public JwtService(IConfiguration config)
        {
            _key = config["Jwt:Key"];
        }
        public string GenerateToken(string userId, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
