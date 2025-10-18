using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stefanini.Application.DTOs.Auth;
using Stefanini.Application.UseCases.Auth;
using Asp.Versioning;

namespace Stefanini.Presentation.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly RefreshTokenUseCase _refreshTokenUseCase;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        LoginUseCase loginUseCase,
        RefreshTokenUseCase refreshTokenUseCase,
        ILogger<AuthController> logger)
    {
        _loginUseCase = loginUseCase;
        _refreshTokenUseCase = refreshTokenUseCase;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _loginUseCase.ExecuteAsync(request, cancellationToken);

            if (response == null)
            {
                return Unauthorized(new { message = "Email ou senha inválidos." });
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var accessToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            
            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized(new { message = "Access token não fornecido." });
            }

            var response = await _refreshTokenUseCase.ExecuteAsync(accessToken, request.RefreshToken, cancellationToken);

            if (response == null)
            {
                return Unauthorized(new { message = "Refresh token inválido ou expirado." });
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao renovar token");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        return Ok(new UserInfoDto
        {
            Id = Guid.Parse(userId!),
            Email = email!,
            NomeCompleto = name!
        });
    }
}

