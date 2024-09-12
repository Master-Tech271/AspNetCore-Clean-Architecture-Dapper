using Api.Application.Common;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEnumExtensions _enumExtension;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions, IEnumExtensions enumExtension)
        {
            _jwtSettings = jwtOptions.Value;
            _dateTimeProvider = dateTimeProvider;
            _enumExtension = enumExtension;
        }

        public string GenerateToken(User user, EnumUserRole role)
        {
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)), SecurityAlgorithms.HmacSha256Signature);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.Username}"),
                new Claim(ClaimTypes.Role, _enumExtension.GetEnumInfo(role).Name)
            };

            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: _dateTimeProvider.UtcNow.AddDays(_jwtSettings.ExpiryInMinutes),
                claims: claims,
                signingCredentials: signingCredentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}
