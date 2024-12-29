using App.Core.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;


        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> Authenticate(int userId, string userEmail, string FirstName, string LastName, string Roles)
        {
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(150); // Token is valid for 30 minutes

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email,userEmail),
                     new Claim("UserId",userId.ToString()),
                     new Claim("Name",FirstName),
                     new Claim("Email",LastName),
                     new Claim(ClaimTypes.Role,Roles),

                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature),


            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescripter);

            var accessToken = tokenHandler.WriteToken(securityToken);

            return accessToken;
        }
    }
}
