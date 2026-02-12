using Dotnet_API_10_.Data;
using Dotnet_API_10_.Dtos;
using Dotnet_API_10_.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dotnet_API_10_.Services
{
    public class AuthService(AuthDbContext _context , IConfiguration configuration) : IAuthService
    {
        public async Task<string?> LoginAsync(USerDto request)
        {
           var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);

            if(user is null)
            {
                return null;
            }

            if (user.UserName != request.UserName)
            {
                return null;
            }
            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            string token = CreateToken(user);

            return token;
        }

        public async Task<User?> RegisterAsync(USerDto request)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == request.UserName))
            {
                return null;
            }
            var user = new User();

            var HashPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.UserName = request.UserName;
            user.PasswordHash = HashPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer:configuration.GetValue<string>("AppSettings:Issuer"),
                audience:configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires:DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
