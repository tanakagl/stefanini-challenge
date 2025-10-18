using Stefanini.Application.DTOs.V2;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Application.UseCases.V2;

public class GetAllUsersUseCaseV2(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserResponseDtoV2>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.ToResponseDtoV2List();
    }
}

