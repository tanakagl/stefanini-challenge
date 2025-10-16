using Backend.Application.DTOs;
using Backend.Application.Mappings;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases;

public class GetAllUsersUseCase(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserResponseDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.ToResponseDtoList();
    }
}

