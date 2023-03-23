using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotnetRpg.Core;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace DotnetRpg.Services.AuthService
{
    public static class AuthUtilities
    {
        public static string CreateHashPassword (string password)
        {
            string passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);
            return passwordHash;
        }

        public static bool VerifyPasswordHash(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public static string GenerateToken(UserProfile userProfile, JwtSettings jwtSettings)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Name, userProfile.UserName),
            };

            Log.Debug("{key}", jwtSettings.SecretKey);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor            
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3), 
                SigningCredentials = creds,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}