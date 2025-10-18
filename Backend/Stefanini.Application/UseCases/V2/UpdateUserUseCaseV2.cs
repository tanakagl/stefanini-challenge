using Stefanini.Application.DTOs.V2;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Application.UseCases.V2;

public class UpdateUserUseCaseV2(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<UserResponseDtoV2?> ExecuteAsync(Guid id, UserUpdateDtoV2 dto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return null;
        }
        
        user.UpdateFromDtoV2(dto);
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        return user.ToResponseDtoV2();
    }
}

