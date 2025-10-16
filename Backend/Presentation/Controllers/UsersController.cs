using Backend.Application.DTOs;
using Backend.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
    CreateUserUseCase createUserUseCase,
    GetAllUsersUseCase getAllUsersUseCase,
    GetUserByNameUseCase getUserByNameUseCase,
    ILogger<UsersController> logger) : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase = createUserUseCase;
    private readonly GetAllUsersUseCase _getAllUsersUseCase = getAllUsersUseCase;
    private readonly GetUserByNameUseCase _getUserByNameUseCase = getUserByNameUseCase;
    private readonly ILogger<UsersController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var users = await _getAllUsersUseCase.ExecuteAsync(cancellationToken);
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuários");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByName([FromQuery] string nome, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _getUserByNameUseCase.ExecuteAsync(nome, cancellationToken);
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuários por nome");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] UserCreateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var createdUser = await _createUserUseCase.ExecuteAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { id = createdUser.Id }, createdUser);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

}

