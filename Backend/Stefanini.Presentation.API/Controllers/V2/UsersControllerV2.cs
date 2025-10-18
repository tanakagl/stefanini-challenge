using Stefanini.Application.DTOs.V2;
using Stefanini.Application.UseCases;
using Stefanini.Application.UseCases.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace Stefanini.Presentation.API.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class UsersControllerV2(
    CreateUserUseCaseV2 createUserUseCase,
    GetAllUsersUseCaseV2 getAllUsersUseCase,
    GetUserByNameUseCaseV2 getUserByNameUseCase,
    DeleteUserUseCase deleteUserUseCase,
    UpdateUserUseCaseV2 updateUserUseCase,
    ILogger<UsersControllerV2> logger) : ControllerBase
{
    private readonly CreateUserUseCaseV2 _createUserUseCase = createUserUseCase;
    private readonly GetAllUsersUseCaseV2 _getAllUsersUseCase = getAllUsersUseCase;
    private readonly GetUserByNameUseCaseV2 _getUserByNameUseCase = getUserByNameUseCase;
    private readonly DeleteUserUseCase _deleteUserUseCase = deleteUserUseCase;
    private readonly UpdateUserUseCaseV2 _updateUserUseCase = updateUserUseCase;
    private readonly ILogger<UsersControllerV2> _logger = logger;

    [HttpGet(Name = "GetAllUsersV2")]
    [ProducesResponseType(typeof(IEnumerable<UserResponseDtoV2>), StatusCodes.Status200OK)]
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

    [HttpGet("search", Name = "GetUsersByNameV2")]
    [ProducesResponseType(typeof(IEnumerable<UserResponseDtoV2>), StatusCodes.Status200OK)]
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

    [HttpPost(Name = "CreateUserV2")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponseDtoV2), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] UserCreateDtoV2 dto, CancellationToken cancellationToken)
    {
        try
        {
            var createdUser = await _createUserUseCase.ExecuteAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { id = createdUser.Id, version = "2.0" }, createdUser);
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

    [HttpPut("{id}", Name = "UpdateUserV2")]
    [ProducesResponseType(typeof(UserResponseDtoV2), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateDtoV2 dto, CancellationToken cancellationToken)
    {
        try
        {
            var updatedUser = await _updateUserUseCase.ExecuteAsync(id, dto, cancellationToken);
            
            if (updatedUser is null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }
            
            return Ok(updatedUser);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpDelete("{id}", Name = "DeleteUserV2")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _deleteUserUseCase.ExecuteAsync(id, cancellationToken);
            
            if (!deleted)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar usuário");
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}

