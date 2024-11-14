using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Reactivities.Application.Configuration;
using Reactivities.Domain.Account.Interfaces;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthConfiguration _config;

        public TokenService(IOptions<AuthConfiguration> options)
        {
            _config = options.Value;
        }

        public string GenerateAccessToken(AppUser user)
        {
            if (user.UserName == null)
            {
                throw new ArgumentNullException(nameof(user), "Username can't be null");
            }
            if (user.Email == null)
            {
                throw new ArgumentNullException(nameof(user), "Email can't be null");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.AccessTokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow + _config.AccessTokenLifetime,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
