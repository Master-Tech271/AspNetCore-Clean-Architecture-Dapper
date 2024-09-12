using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Application.Common
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, EnumUserRole role);
    }
}
