using System.Security.Claims;
using Stefanini.Domain.Entities;

namespace Stefanini.Application.Services;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    DateTime GetTokenExpirationTime();
}

