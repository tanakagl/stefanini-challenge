using Stefanini.Application.DTOs.Auth;
using Stefanini.Application.Services;
using Stefanini.Domain.Interfaces;
using System.Security.Claims;

namespace Stefanini.Application.UseCases.Auth;

public class RefreshTokenUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public RefreshTokenUseCase(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto?> ExecuteAsync(
        string accessToken,
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        // Validar access token expirado
        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return null;
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return null;
        }

        // Buscar usuário
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null ||
            user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null; // Refresh token inválido ou expirado
        }

        // Gerar novos tokens
        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        var expiresAt = _jwtService.GetTokenExpirationTime();

        // Atualizar refresh token no banco
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = expiresAt,
            User = new UserInfoDto
            {
                Id = user.Id,
                NomeCompleto = user.NomeCompleto,
                Email = user.Email
            }
        };
    }
}

