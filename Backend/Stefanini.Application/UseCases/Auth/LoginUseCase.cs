using BCrypt.Net;
using Stefanini.Application.DTOs.Auth;
using Stefanini.Application.Services;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginUseCase(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto?> ExecuteAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        // Buscar usuário por email
        var users = await _userRepository.FindAsync(u => u.Email == request.Email, cancellationToken);
        var user = users.FirstOrDefault();

        if (user == null || string.IsNullOrEmpty(user.PasswordHash))
        {
            return null; // Usuário não encontrado ou sem senha
        }

        // Verificar senha
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null; // Senha incorreta
        }

        // Gerar tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var expiresAt = _jwtService.GetTokenExpirationTime();

        // Salvar refresh token no banco
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Refresh token válido por 7 dias

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
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

