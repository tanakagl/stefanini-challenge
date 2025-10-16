using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases;

/// <summary>
/// Use Case para listar todos os usuários
/// </summary>
public class GetAllUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }
}

