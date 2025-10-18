using Stefanini.Application.DTOs;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Application.UseCases;

public class GetAllUsersUseCase(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserResponseDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.ToResponseDtoList();
    }
}

